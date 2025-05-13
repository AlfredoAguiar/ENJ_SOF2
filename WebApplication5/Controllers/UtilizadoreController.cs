using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadoreController : ControllerBase
    {
        private readonly Dbes2 _context;
        private readonly IPasswordHasher<Utilizadore> _passwordHasher;
 
        public UtilizadoreController(Dbes2 context, IPasswordHasher<Utilizadore> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        // GET: api/Utilizadore
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UtilizadoreDto>>> GetUtilizadores()
        {
            var utilizadores = await _context.Utilizadores.Include(u => u.Permissao).ToListAsync();

            var utilizadoreDtos = utilizadores.Select(u => new UtilizadoreDto
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Cargo = u.Cargo,
                PermissaoId = u.PermissaoId
            }).ToList();

            return Ok(utilizadoreDtos);
        }

        // GET: api/Utilizadore/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UtilizadoreDto>> GetUtilizadore(Guid id)
        {
            var utilizadore = await _context.Utilizadores.Include(u => u.Permissao)
                                                         .FirstOrDefaultAsync(u => u.Id == id);

            if (utilizadore == null)
            {
                return NotFound();
            }
            
            var utilizadoreDto = new UtilizadoreDto
            {
                Id = utilizadore.Id,
                Nome = utilizadore.Nome,
                Email = utilizadore.Email,
                Cargo = utilizadore.Cargo,
                PermissaoId = utilizadore.PermissaoId
            };

            return utilizadoreDto;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilizadore(Guid id, UtilizadoreDto utilizadoreDto)
        {
            if (id != utilizadoreDto.Id)
            {
                return BadRequest("IDs não coincidem.");
            }

            // Buscando o usuário no banco de dados
            var utilizadore = await _context.Utilizadores.FindAsync(id);
            if (utilizadore == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Atualizando as propriedades
            utilizadore.Nome = utilizadoreDto.Nome;
            utilizadore.Email = utilizadoreDto.Email;
            utilizadore.Cargo = utilizadoreDto.Cargo;
            utilizadore.PermissaoId = utilizadoreDto.PermissaoId;

            // Atualiza a senha se ela for fornecida
            if (!string.IsNullOrEmpty(utilizadoreDto.Senha))
            {
                utilizadore.Senha = _passwordHasher.HashPassword(utilizadore, utilizadoreDto.Senha);
            }

            // Marcando a entidade como modificada (opcional, pois já estamos trabalhando com a entidade existente)
            _context.Entry(utilizadore).State = EntityState.Modified;

            try
            {
                // Salvando as mudanças no banco de dados
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verifica se a entidade ainda existe
                if (!UtilizadoreExists(id))
                {
                    return NotFound("Usuário não encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Retorna sucesso sem conteúdo
        }


        // POST: api/Utilizadore
        [HttpPost]
        public async Task<ActionResult<UtilizadoreDto>> PostUtilizadore(UtilizadoreDto utilizadoreDto)
        {
            // Map DTO to the Utilizadore model
            var utilizadore = new Utilizadore
            {
                Id = Guid.NewGuid(),
                Nome = utilizadoreDto.Nome,
                Email = utilizadoreDto.Email,
                Senha = utilizadoreDto.Senha,
                Cargo = utilizadoreDto.Cargo,
                PermissaoId = utilizadoreDto.PermissaoId 
            };
            utilizadore.Senha = _passwordHasher.HashPassword(utilizadore, utilizadoreDto.Senha);
            _context.Utilizadores.Add(utilizadore);
            await _context.SaveChangesAsync();
            
            var createdUtilizadoreDto = new UtilizadoreDto
            {
                Id = utilizadore.Id,
                Nome = utilizadore.Nome,
                Email = utilizadore.Email,
                Senha = utilizadore.Senha,
                Cargo = utilizadore.Cargo,
                PermissaoId = utilizadore.PermissaoId 
            };

            return CreatedAtAction("GetUtilizadore", new { id = createdUtilizadoreDto.Id }, createdUtilizadoreDto);
        }

        // DELETE: api/Utilizadore/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilizadore(Guid id)
        {
            var utilizadore = await _context.Utilizadores.FindAsync(id);
            if (utilizadore == null)
            {
                return NotFound();
            }

            _context.Utilizadores.Remove(utilizadore);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UtilizadoreExists(Guid id)
        {
            return _context.Utilizadores.Any(e => e.Id == id);
        }
    
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] WebApplication5.DTO.LoginRequest request)
        {
            var user = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized("Email ou senha inválidos.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Senha, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Email ou senha inválidos.");
            }

            var response = new LoginResponse
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Nome
            };

            return Ok(response);
        }

    
    }
}



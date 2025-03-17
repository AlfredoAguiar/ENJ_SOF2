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

        public UtilizadoreController(Dbes2 context)
        {
            _context = context;
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
                Senha = u.Senha,
                Cargo = u.Cargo,
                PermissaoId = u.PermissaoId
            }).ToList();

            return utilizadoreDtos;
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
                Senha = utilizadore.Senha,
                Cargo = utilizadore.Cargo,
                PermissaoId = utilizadore.PermissaoId
            };

            return utilizadoreDto;
        }

        // PUT: api/Utilizadore/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilizadore(Guid id, UtilizadoreDto utilizadoreDto)
        {
            if (id != utilizadoreDto.Id)
            {
                return BadRequest();
            }
            
            var utilizadore = new Utilizadore
            {
                Id = utilizadoreDto.Id,
                Nome = utilizadoreDto.Nome,
                Email = utilizadoreDto.Email,
                Senha = utilizadoreDto.Senha,
                Cargo = utilizadoreDto.Cargo,
                PermissaoId = utilizadoreDto.PermissaoId 
            };

            _context.Entry(utilizadore).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilizadoreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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
    }
}



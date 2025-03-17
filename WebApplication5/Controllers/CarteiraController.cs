using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarteiraController : ControllerBase
    {
        private readonly Dbes2 _context;

        public CarteiraController(Dbes2 context)
        {
            _context = context;
        }

        // GET: api/Carteira
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarteiraDto>>> GetCarteiras()
        {
            var carteiras = await _context.Carteiras
                .Include(c => c.Utilizador) // Incluindo a navegação do Utilizador
                .Include(c => c.Ativo) // Incluindo a navegação do Ativo
                .Select(c => new CarteiraDto
                {
                    Id = c.Id,
                    UtilizadorId = c.UtilizadorId,
                    AtivoId = c.AtivoId,
                    DataInicio = c.DataInicio.ToDateTime(TimeOnly.MinValue), // Converte DateOnly para DateTime
                    Montante = c.Montante,
                    UtilizadorNome = c.Utilizador != null ? c.Utilizador.Nome : null, // Usando o operador ternário para evitar erro
                    AtivoTipo = c.Ativo != null ? c.Ativo.Tipo : null // Usando o operador ternário para evitar erro
                })
                .ToListAsync();

            return carteiras;
        }

        // GET: api/Carteira/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CarteiraDto>> GetCarteira(Guid id)
        {
            var carteira = await _context.Carteiras
                .Include(c => c.Utilizador)
                .Include(c => c.Ativo)
                .Where(c => c.Id == id)
                .Select(c => new CarteiraDto
                {
                    Id = c.Id,
                    UtilizadorId = c.UtilizadorId,
                    AtivoId = c.AtivoId,
                    DataInicio = c.DataInicio.ToDateTime(TimeOnly.MinValue), // Converte DateOnly para DateTime
                    Montante = c.Montante,
                    UtilizadorNome = c.Utilizador != null ? c.Utilizador.Nome : null, // Usando o operador ternário
                    AtivoTipo = c.Ativo != null ? c.Ativo.Tipo : null // Usando o operador ternário
                })
                .FirstOrDefaultAsync();

            if (carteira == null)
            {
                return NotFound();
            }

            return carteira;
        }

        // POST: api/Carteira
        [HttpPost]
        public async Task<ActionResult<CarteiraDto>> PostCarteira(CarteiraDto carteiraDto)
        {
            var carteira = new Carteira
            {
                Id = Guid.NewGuid(),
                UtilizadorId = carteiraDto.UtilizadorId,
                AtivoId = carteiraDto.AtivoId,
                DataInicio = DateOnly.FromDateTime(carteiraDto.DataInicio), // Converte DateTime para DateOnly
                Montante = carteiraDto.Montante
            };

            _context.Carteiras.Add(carteira);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarteira", new { id = carteira.Id }, carteiraDto);
        }

        // PUT: api/Carteira/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarteira(Guid id, CarteiraDto carteiraDto)
        {
            if (id != carteiraDto.Id)
            {
                return BadRequest();
            }

            var carteira = await _context.Carteiras.FindAsync(id);
            if (carteira == null)
            {
                return NotFound();
            }

            carteira.UtilizadorId = carteiraDto.UtilizadorId;
            carteira.AtivoId = carteiraDto.AtivoId;
            carteira.DataInicio = DateOnly.FromDateTime(carteiraDto.DataInicio); // Converte DateTime para DateOnly
            carteira.Montante = carteiraDto.Montante;

            _context.Entry(carteira).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarteiraExists(id))
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

        // DELETE: api/Carteira/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarteira(Guid id)
        {
            var carteira = await _context.Carteiras.FindAsync(id);
            if (carteira == null)
            {
                return NotFound();
            }

            _context.Carteiras.Remove(carteira);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarteiraExists(Guid id)
        {
            return _context.Carteiras.Any(e => e.Id == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtivosFinanceirosController : ControllerBase
    {
        private readonly Dbes2 _context;

        public AtivosFinanceirosController(Dbes2 context)
        {
            _context = context;
        }

        // GET: api/AtivosFinanceiros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AtivosFinanceiroDto>>> GetAtivosFinanceiros()
        {
            var ativos = await _context.AtivosFinanceiros
                .Select(a => new AtivosFinanceiroDto
                {
                    Id = a.Id,
                    Tipo = a.Tipo,
                    DataInicio = a.DataInicio,
                    TaxaPercentagem = a.TaxaPercentagem
                })
                .ToListAsync();

            return Ok(ativos);
        }

        // GET: api/AtivosFinanceiros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AtivosFinanceiroDto>> GetAtivosFinanceiro(Guid id)
        {
            var ativo = await _context.AtivosFinanceiros
                .Where(a => a.Id == id)
                .Select(a => new AtivosFinanceiroDto
                {
                    Id = a.Id,
                    Tipo = a.Tipo,
                    DataInicio = a.DataInicio,
                    TaxaPercentagem = a.TaxaPercentagem
                })
                .FirstOrDefaultAsync();

            if (ativo == null)
            {
                return NotFound();
            }

            return Ok(ativo);
        }

        // POST: api/AtivosFinanceiros
        [HttpPost]
        public async Task<ActionResult<AtivosFinanceiroDto>> PostAtivosFinanceiro(AtivosFinanceiroDto ativoDto)
        {
            var ativo = new AtivosFinanceiro
            {
                Id = ativoDto.Id,
                Tipo = ativoDto.Tipo,
                DataInicio = ativoDto.DataInicio,
                TaxaPercentagem = ativoDto.TaxaPercentagem
            };

            _context.AtivosFinanceiros.Add(ativo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAtivosFinanceiro), new { id = ativo.Id }, ativoDto);
        }

        // PUT: api/AtivosFinanceiros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAtivosFinanceiro(Guid id, AtivosFinanceiroDto ativoDto)
        {
            if (id != ativoDto.Id)
            {
                return BadRequest();
            }

            var ativo = await _context.AtivosFinanceiros.FindAsync(id);
            if (ativo == null)
            {
                return NotFound();
            }

            ativo.Tipo = ativoDto.Tipo;
            ativo.DataInicio = ativoDto.DataInicio;
            ativo.TaxaPercentagem = ativoDto.TaxaPercentagem;

            _context.Entry(ativo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AtivosFinanceiroExists(id))
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

        // DELETE: api/AtivosFinanceiros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAtivosFinanceiro(Guid id)
        {
            var ativo = await _context.AtivosFinanceiros.FindAsync(id);
            if (ativo == null)
            {
                return NotFound();
            }

            _context.AtivosFinanceiros.Remove(ativo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AtivosFinanceiroExists(Guid id)
        {
            return _context.AtivosFinanceiros.Any(e => e.Id == id);
        }
    }
}

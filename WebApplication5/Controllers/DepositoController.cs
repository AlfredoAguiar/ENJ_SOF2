// Controllers/DepositoController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositoController : ControllerBase
    {
        private readonly Dbes2 _context;

        public DepositoController(Dbes2 context)
        {
            _context = context;
        }

        // GET: api/Deposito
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepositoDto>>> GetDepositos()
        {
            var depositos = await _context.Depositos
                .Include(d => d.Ativo)
                .Select(d => new DepositoDto
                {
                    Id = d.Id,
                    AtivoId = d.AtivoId,
                    Montante = d.Montante,
                    Banco = d.Banco,
                    NumeroConta = d.NumeroConta,
                    TaxaJuros = d.TaxaJuros,
                    Titulares = d.Titulares,
                    AtivoTipo = d.Ativo != null ? d.Ativo.Tipo : null
                })
                .ToListAsync();

            return Ok(depositos);
        }

        // GET: api/Deposito/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepositoDto>> GetDeposito(Guid id)
        {
            var deposito = await _context.Depositos
                .Include(d => d.Ativo)
                .Where(d => d.Id == id)
                .Select(d => new DepositoDto
                {
                    Id = d.Id,
                    AtivoId = d.AtivoId,
                    Montante = d.Montante,
                    Banco = d.Banco,
                    NumeroConta = d.NumeroConta,
                    TaxaJuros = d.TaxaJuros,
                    Titulares = d.Titulares,
                    //  propriedade adicional do DTO
                    AtivoTipo = d.Ativo != null ? d.Ativo.Tipo : null
                })
                .FirstOrDefaultAsync();

            if (deposito == null)
            {
                return NotFound();
            }

            return deposito;
        }

        // POST: api/Deposito
        [HttpPost]
        public async Task<ActionResult<DepositoDto>> PostDeposito(DepositoDto depositoDto)
        {
            var deposito = new Deposito
            {
                Id = Guid.NewGuid(),
                AtivoId = depositoDto.AtivoId,
                Montante = depositoDto.Montante,
                Banco = depositoDto.Banco,
                NumeroConta = depositoDto.NumeroConta,
                TaxaJuros = depositoDto.TaxaJuros,
                Titulares = depositoDto.Titulares
            };

            _context.Depositos.Add(deposito);
            await _context.SaveChangesAsync();

            // DTO após a criação
            return CreatedAtAction("GetDeposito", new { id = deposito.Id }, depositoDto);
        }

        // PUT: api/Deposito/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeposito(Guid id, DepositoDto depositoDto)
        {
            if (id != depositoDto.Id)
            {
                return BadRequest();
            }

            var deposito = await _context.Depositos.FindAsync(id);
            if (deposito == null)
            {
                return NotFound();
            }

            deposito.AtivoId = depositoDto.AtivoId;
            deposito.Montante = depositoDto.Montante;
            deposito.Banco = depositoDto.Banco;
            deposito.NumeroConta = depositoDto.NumeroConta;
            deposito.TaxaJuros = depositoDto.TaxaJuros;
            deposito.Titulares = depositoDto.Titulares;

            _context.Entry(deposito).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepositoExists(id))
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

        // DELETE: api/Deposito/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeposito(Guid id)
        {
            var deposito = await _context.Depositos.FindAsync(id);
            if (deposito == null)
            {
                return NotFound();
            }

            _context.Depositos.Remove(deposito);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepositoExists(Guid id)
        {
            return _context.Depositos.Any(e => e.Id == id);
        }
    }
}

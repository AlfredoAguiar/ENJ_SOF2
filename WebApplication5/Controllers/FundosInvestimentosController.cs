using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundosInvestimentosController : ControllerBase
    {
        private readonly Dbes2 _context;

        public FundosInvestimentosController(Dbes2 context)
        {
            _context = context;
        }

        // GET: api/FundosInvestimentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FundosInvestimentoDto>>> GetFundosInvestimentos()
        {
            var fundos = await _context.FundosInvestimentos
                .Select(f => new FundosInvestimentoDto
                {
                    Id = f.Id,
                    AtivoId = f.AtivoId,
                    Nome = f.Nome,
                    MontanteInvestido = f.MontanteInvestido,
                    DuracaoMeses = f.DuracaoMeses,
                    TaxaJurosPadrao = f.TaxaJurosPadrao
                })
                .ToListAsync();

            return Ok(fundos);
        }

        // GET: api/FundosInvestimentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FundosInvestimentoDto>> GetFundosInvestimento(Guid id)
        {
            var fundo = await _context.FundosInvestimentos
                .Where(f => f.Id == id)
                .Select(f => new FundosInvestimentoDto
                {
                    Id = f.Id,
                    AtivoId = f.AtivoId,
                    Nome = f.Nome,
                    MontanteInvestido = f.MontanteInvestido,
                    DuracaoMeses = f.DuracaoMeses,
                    TaxaJurosPadrao = f.TaxaJurosPadrao
                })
                .FirstOrDefaultAsync();

            if (fundo == null)
            {
                return NotFound();
            }

            return Ok(fundo);
        }

        // POST: api/FundosInvestimentos
        [HttpPost]
        public async Task<ActionResult<FundosInvestimentoDto>> PostFundosInvestimento(FundosInvestimentoDto fundoDto)
        {
            var fundo = new FundosInvestimento
            {
                Id = Guid.NewGuid(),
                AtivoId = fundoDto.AtivoId,
                Nome = fundoDto.Nome,
                MontanteInvestido = fundoDto.MontanteInvestido,
                DuracaoMeses = fundoDto.DuracaoMeses,
                TaxaJurosPadrao = fundoDto.TaxaJurosPadrao
            };

            _context.FundosInvestimentos.Add(fundo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFundosInvestimento), new { id = fundo.Id }, fundoDto);
        }

        // PUT: api/FundosInvestimentos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFundosInvestimento(Guid id, FundosInvestimentoDto fundoDto)
        {
            if (id != fundoDto.Id)
            {
                return BadRequest();
            }

            var fundo = await _context.FundosInvestimentos.FindAsync(id);
            if (fundo == null)
            {
                return NotFound();
            }

            fundo.AtivoId = fundoDto.AtivoId;
            fundo.Nome = fundoDto.Nome;
            fundo.MontanteInvestido = fundoDto.MontanteInvestido;
            fundo.DuracaoMeses = fundoDto.DuracaoMeses;
            fundo.TaxaJurosPadrao = fundoDto.TaxaJurosPadrao;

            _context.Entry(fundo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FundosInvestimentoExists(id))
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

        // DELETE: api/FundosInvestimentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFundosInvestimento(Guid id)
        {
            var fundo = await _context.FundosInvestimentos.FindAsync(id);
            if (fundo == null)
            {
                return NotFound();
            }

            _context.FundosInvestimentos.Remove(fundo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FundosInvestimentoExists(Guid id)
        {
            return _context.FundosInvestimentos.Any(e => e.Id == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public enum TipoObjeto
    {
        Carteira,
        AtivoFinanceiro
    }

    public class FinanceDto
    {
        public TipoObjeto TipoObjeto { get; set; }

        // Propriedades Carteira
        public Guid? Id { get; set; }
        public Guid? UtilizadorId { get; set; }
        public Guid? AtivoId { get; set; }
        public DateTime? DataInicio { get; set; }
        public double? Montante { get; set; }
        public string? UtilizadorNome { get; set; }
        public string? AtivoTipo { get; set; }

        // Propriedades AtivoFinanceiro
        public string? Tipo { get; set; }
        public double? TaxaPercentagem { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly Dbes2 _context;

        public FinanceController(Dbes2 context)
        {
            _context = context;
        }

        // GET api/finance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinanceDto>>> GetTodos()
        {
            var carteiras = await _context.Carteiras
                .Include(c => c.Utilizador)
                .Include(c => c.Ativo)
                .Select(c => new FinanceDto
                {
                    TipoObjeto = TipoObjeto.Carteira,
                    Id = c.Id,
                    UtilizadorId = c.UtilizadorId,
                    AtivoId = c.AtivoId,
                    DataInicio = c.DataInicio.ToDateTime(TimeOnly.MinValue),
                    Montante = c.Montante,
                    UtilizadorNome = c.Utilizador.Nome,
                    AtivoTipo = c.Ativo.Tipo
                }).ToListAsync();

            var ativos = await _context.AtivosFinanceiros
                .Select(a => new FinanceDto
                {
                    TipoObjeto = TipoObjeto.AtivoFinanceiro,
                    Id = a.Id,
                    Tipo = a.Tipo,
                    DataInicio = a.DataInicio.ToDateTime(TimeOnly.MinValue),
                    TaxaPercentagem = a.TaxaPercentagem
                }).ToListAsync();

            // Junta as listas
            var todos = carteiras.Cast<FinanceDto>().Concat(ativos).ToList();

            return Ok(todos);
        }

        // GET api/finance/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FinanceDto>> GetPorId(Guid id)
        {
            var carteira = await _context.Carteiras
                .Include(c => c.Utilizador)
                .Include(c => c.Ativo)
                .Where(c => c.Id == id)
                .Select(c => new FinanceDto
                {
                    TipoObjeto = TipoObjeto.Carteira,
                    Id = c.Id,
                    UtilizadorId = c.UtilizadorId,
                    AtivoId = c.AtivoId,
                    DataInicio = c.DataInicio.ToDateTime(TimeOnly.MinValue),
                    Montante = c.Montante,
                    UtilizadorNome = c.Utilizador.Nome,
                    AtivoTipo = c.Ativo.Tipo
                })
                .FirstOrDefaultAsync();

            if (carteira != null) return carteira;

            var ativo = await _context.AtivosFinanceiros
                .Where(a => a.Id == id)
                .Select(a => new FinanceDto
                {
                    TipoObjeto = TipoObjeto.AtivoFinanceiro,
                    Id = a.Id,
                    Tipo = a.Tipo,
                    DataInicio = a.DataInicio.ToDateTime(TimeOnly.MinValue),
                    TaxaPercentagem = a.TaxaPercentagem
                })
                .FirstOrDefaultAsync();

            if (ativo != null) return ativo;

            return NotFound();
        }

        // POST api/finance
        [HttpPost]
        public async Task<ActionResult<FinanceDto>> Post(FinanceDto dto)
        {
            if (dto.TipoObjeto == TipoObjeto.Carteira)
            {
                var carteira = new Carteira
                {
                    Id = Guid.NewGuid(),
                    UtilizadorId = dto.UtilizadorId ?? Guid.Empty,
                    AtivoId = dto.AtivoId ?? Guid.Empty,
                    DataInicio = DateOnly.FromDateTime(dto.DataInicio ?? DateTime.Now),
                    Montante = dto.Montante ?? 0
                };

                _context.Carteiras.Add(carteira);
                await _context.SaveChangesAsync();

                dto.Id = carteira.Id;
                return CreatedAtAction(nameof(GetPorId), new { id = carteira.Id }, dto);
            }
            else if (dto.TipoObjeto == TipoObjeto.AtivoFinanceiro)
            {
                var ativo = new AtivosFinanceiro
                {
                    Id = Guid.NewGuid(),
                    Tipo = dto.Tipo,
                    DataInicio = DateOnly.FromDateTime(dto.DataInicio ?? DateTime.Now),
                    TaxaPercentagem = dto.TaxaPercentagem ?? 0
                };

                _context.AtivosFinanceiros.Add(ativo);
                await _context.SaveChangesAsync();

                dto.Id = ativo.Id;
                return CreatedAtAction(nameof(GetPorId), new { id = ativo.Id }, dto);
            }
            else
            {
                return BadRequest("TipoObjeto inválido");
            }
        }

        // PUT api/finance/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, FinanceDto dto)
        {
            if (id != dto.Id) return BadRequest();

            if (dto.TipoObjeto == TipoObjeto.Carteira)
            {
                var carteira = await _context.Carteiras.FindAsync(id);
                if (carteira == null) return NotFound();

                carteira.UtilizadorId = dto.UtilizadorId ?? carteira.UtilizadorId;
                carteira.AtivoId = dto.AtivoId ?? carteira.AtivoId;
                carteira.DataInicio = DateOnly.FromDateTime(dto.DataInicio ?? carteira.DataInicio.ToDateTime(TimeOnly.MinValue));
                carteira.Montante = dto.Montante ?? carteira.Montante;

                _context.Entry(carteira).State = EntityState.Modified;
            }
            else if (dto.TipoObjeto == TipoObjeto.AtivoFinanceiro)
            {
                var ativo = await _context.AtivosFinanceiros.FindAsync(id);
                if (ativo == null) return NotFound();

                ativo.Tipo = dto.Tipo ?? ativo.Tipo;
                ativo.DataInicio = DateOnly.FromDateTime(dto.DataInicio ?? ativo.DataInicio.ToDateTime(TimeOnly.MinValue));
                ativo.TaxaPercentagem = dto.TaxaPercentagem ?? ativo.TaxaPercentagem;

                _context.Entry(ativo).State = EntityState.Modified;
            }
            else
            {
                return BadRequest("TipoObjeto inválido");
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = dto.TipoObjeto == TipoObjeto.Carteira
                    ? _context.Carteiras.Any(e => e.Id == id)
                    : _context.AtivosFinanceiros.Any(e => e.Id == id);

                if (!exists) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE api/finance/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var carteira = await _context.Carteiras.FindAsync(id);
            if (carteira != null)
            {
                _context.Carteiras.Remove(carteira);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            var ativo = await _context.AtivosFinanceiros.FindAsync(id);
            if (ativo != null)
            {
                _context.AtivosFinanceiros.Remove(ativo);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            return NotFound();
        }
    }
}

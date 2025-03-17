// Controllers/ImoveisArrendamentoController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImoveisArrendamentoController : ControllerBase
    {
        private readonly Dbes2 _context;

        public ImoveisArrendamentoController(Dbes2 context)
        {
            _context = context;
        }

        // GET: api/ImoveisArrendamento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImoveisArrendamentoDto>>> GetImoveisArrendamento()
        {
            var imoveis = await _context.ImoveisArrendamentos
                .Include(i => i.Ativo)
                .Include(i => i.Localizacao)
                .Select(i => new ImoveisArrendamentoDto
                {
                    Id = i.Id,
                    AtivoId = i.AtivoId,
                    Designacao = i.Designacao,
                    LocalizacaoId = i.LocalizacaoId,
                    ValorPropriedade = i.ValorPropriedade,
                    ValorRenda = i.ValorRenda,
                    TaxaCondominio = i.TaxaCondominio,
                    DespesasAnuais = i.DespesasAnuais,
                    PercentagemPropriedade = i.PercentagemPropriedade,
                    // Preenchendo as propriedades adicionais do DTO
                    AtivoTipo = i.Ativo != null ? i.Ativo.Tipo : null,
                    LocalizacaoCidade = i.Localizacao != null ? i.Localizacao.Cidade : null
                })
                .ToListAsync();

            return imoveis;
        }

        // GET: api/ImoveisArrendamento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImoveisArrendamentoDto>> GetImoveisArrendamento(Guid id)
        {
            var imovel = await _context.ImoveisArrendamentos
                .Include(i => i.Ativo)
                .Include(i => i.Localizacao)
                .Where(i => i.Id == id)
                .Select(i => new ImoveisArrendamentoDto
                {
                    Id = i.Id,
                    AtivoId = i.AtivoId,
                    Designacao = i.Designacao,
                    LocalizacaoId = i.LocalizacaoId,
                    ValorPropriedade = i.ValorPropriedade,
                    ValorRenda = i.ValorRenda,
                    TaxaCondominio = i.TaxaCondominio,
                    DespesasAnuais = i.DespesasAnuais,
                    PercentagemPropriedade = i.PercentagemPropriedade,
                    // Preenchendo as propriedades adicionais do DTO
                    AtivoTipo = i.Ativo != null ? i.Ativo.Tipo : null,
                    LocalizacaoCidade = i.Localizacao != null ? i.Localizacao.Cidade : null
                })
                .FirstOrDefaultAsync();

            if (imovel == null)
            {
                return NotFound();
            }

            return imovel;
        }

        // POST: api/ImoveisArrendamento
        [HttpPost]
        public async Task<ActionResult<ImoveisArrendamentoDto>> PostImoveisArrendamento(ImoveisArrendamentoDto imoveisArrendamentoDto)
        {
            var imovel = new ImoveisArrendamento
            {
                Id = Guid.NewGuid(),
                AtivoId = imoveisArrendamentoDto.AtivoId,
                Designacao = imoveisArrendamentoDto.Designacao,
                LocalizacaoId = imoveisArrendamentoDto.LocalizacaoId,
                ValorPropriedade = imoveisArrendamentoDto.ValorPropriedade,
                ValorRenda = imoveisArrendamentoDto.ValorRenda,
                TaxaCondominio = imoveisArrendamentoDto.TaxaCondominio,
                DespesasAnuais = imoveisArrendamentoDto.DespesasAnuais,
                PercentagemPropriedade = imoveisArrendamentoDto.PercentagemPropriedade
            };

            _context.ImoveisArrendamentos.Add(imovel);
            await _context.SaveChangesAsync();

            // Retornando o DTO após a criação
            return CreatedAtAction("GetImoveisArrendamento", new { id = imovel.Id }, imoveisArrendamentoDto);
        }

        // PUT: api/ImoveisArrendamento/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImoveisArrendamento(Guid id, ImoveisArrendamentoDto imoveisArrendamentoDto)
        {
            if (id != imoveisArrendamentoDto.Id)
            {
                return BadRequest();
            }

            var imovel = await _context.ImoveisArrendamentos.FindAsync(id);
            if (imovel == null)
            {
                return NotFound();
            }

            imovel.AtivoId = imoveisArrendamentoDto.AtivoId;
            imovel.Designacao = imoveisArrendamentoDto.Designacao;
            imovel.LocalizacaoId = imoveisArrendamentoDto.LocalizacaoId;
            imovel.ValorPropriedade = imoveisArrendamentoDto.ValorPropriedade;
            imovel.ValorRenda = imoveisArrendamentoDto.ValorRenda;
            imovel.TaxaCondominio = imoveisArrendamentoDto.TaxaCondominio;
            imovel.DespesasAnuais = imoveisArrendamentoDto.DespesasAnuais;
            imovel.PercentagemPropriedade = imoveisArrendamentoDto.PercentagemPropriedade;

            _context.Entry(imovel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImoveisArrendamentoExists(id))
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

        // DELETE: api/ImoveisArrendamento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImoveisArrendamento(Guid id)
        {
            var imovel = await _context.ImoveisArrendamentos.FindAsync(id);
            if (imovel == null)
            {
                return NotFound();
            }

            _context.ImoveisArrendamentos.Remove(imovel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImoveisArrendamentoExists(Guid id)
        {
            return _context.ImoveisArrendamentos.Any(e => e.Id == id);
        }
    }
}

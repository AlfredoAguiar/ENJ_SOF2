using WebApplication1.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.DTO;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizacoesController : ControllerBase
    {
        private readonly Dbes2 _context;

        public LocalizacoesController(Dbes2 context)
        {
            _context = context;
        }

        // GET: api/Localizacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocalizacoDto>>> GetLocalizacoes()
        {
            var localizacoes = await _context.Localizacoes
                .Select(l => new LocalizacoDto
                {
                    Id = l.Id,
                    Endereco = l.Endereco,
                    Cidade = l.Cidade,
                    Pais = l.Pais,
                    CodigoPostal = l.CodigoPostal
                })
                .ToListAsync();

            return Ok(localizacoes);
        }

        // GET: api/Localizacoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocalizacoDto>> GetLocalizaco(Guid id)
        {
            var localizaco = await _context.Localizacoes
                .Where(l => l.Id == id)
                .Select(l => new LocalizacoDto
                {
                    Id = l.Id,
                    Endereco = l.Endereco,
                    Cidade = l.Cidade,
                    Pais = l.Pais,
                    CodigoPostal = l.CodigoPostal
                })
                .FirstOrDefaultAsync();

            if (localizaco == null)
            {
                return NotFound();
            }

            return Ok(localizaco);
        }

        // POST: api/Localizacoes
        [HttpPost]
        public async Task<ActionResult<LocalizacoDto>> PostLocalizaco(LocalizacoDto localizacoDto)
        {
            var localizaco = new Localizaco
            {
                Id = Guid.NewGuid(),
                Endereco = localizacoDto.Endereco,
                Cidade = localizacoDto.Cidade,
                Pais = localizacoDto.Pais,
                CodigoPostal = localizacoDto.CodigoPostal
            };

            _context.Localizacoes.Add(localizaco);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocalizaco), new { id = localizaco.Id }, localizacoDto);
        }

        // PUT: api/Localizacoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocalizaco(Guid id, LocalizacoDto localizacoDto)
        {
            if (id != localizacoDto.Id)
            {
                return BadRequest();
            }

            var localizaco = await _context.Localizacoes.FindAsync(id);
            if (localizaco == null)
            {
                return NotFound();
            }

            localizaco.Endereco = localizacoDto.Endereco;
            localizaco.Cidade = localizacoDto.Cidade;
            localizaco.Pais = localizacoDto.Pais;
            localizaco.CodigoPostal = localizacoDto.CodigoPostal;

            _context.Entry(localizaco).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalizacoExists(id))
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

        // DELETE: api/Localizacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocalizaco(Guid id)
        {
            var localizaco = await _context.Localizacoes.FindAsync(id);
            if (localizaco == null)
            {
                return NotFound();
            }

            _context.Localizacoes.Remove(localizaco);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocalizacoExists(Guid id)
        {
            return _context.Localizacoes.Any(e => e.Id == id);
        }
    }
}

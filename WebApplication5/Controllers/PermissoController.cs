
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication5.DTO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
[ApiController]
public class PermissoController : ControllerBase
{
    private readonly Dbes2 _context;

    public PermissoController(Dbes2 context)
    {
        _context = context;
    }

    // GET: api/Permisso
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PermissoDto>>> GetPermissoes()
    {
        var permissoes = await _context.Permissoes
            .Select(p => new PermissoDto
            {
                Id = p.Id,
                PodeCriarUtilizadores = p.PodeCriarUtilizadores,
                PodeEditarUtilizadores = p.PodeEditarUtilizadores,
                PodeVerRegistos = p.PodeVerRegistos,
                PodeAlterarPermissoes = p.PodeAlterarPermissoes
            })
            .ToListAsync();

        return permissoes;
    }

    // GET: api/Permisso/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PermissoDto>> GetPermisso(Guid id)
    {
        var permissao = await _context.Permissoes
            .Where(p => p.Id == id)
            .Select(p => new PermissoDto
            {
                Id = p.Id,
                PodeCriarUtilizadores = p.PodeCriarUtilizadores,
                PodeEditarUtilizadores = p.PodeEditarUtilizadores,
                PodeVerRegistos = p.PodeVerRegistos,
                PodeAlterarPermissoes = p.PodeAlterarPermissoes
            })
            .FirstOrDefaultAsync();

        if (permissao == null)
        {
            return NotFound();
        }

        return permissao;
    }

    // PUT: api/Permisso/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePermisso(Guid id, PermissoDto permisssoDto)
    {
        // Find the existing record by Id
        var permissao = await _context.Permissoes.FindAsync(id);
    
        // If not found, return a 404 Not Found
        if (permissao == null)
        {
            return NotFound();
        }

        // Map the updated fields from the DTO to the model
        permissao.PodeCriarUtilizadores = permisssoDto.PodeCriarUtilizadores;
        permissao.PodeEditarUtilizadores = permisssoDto.PodeEditarUtilizadores;
        permissao.PodeVerRegistos = permisssoDto.PodeVerRegistos;
        permissao.PodeAlterarPermissoes = permisssoDto.PodeAlterarPermissoes;

        // Save changes to the database
        await _context.SaveChangesAsync();

        // Return no content (HTTP 204) indicating the update was successful
        return NoContent();
    }

    // POST: api/Permisso
    [HttpPost]
    public async Task<ActionResult<PermissoDto>> CreatePermisso(PermissoDto permisssoDto)
    {
        // Map the DTO to the Model (Entity)
        var permissao = new Permisso
        {
            Id = Guid.NewGuid(),  // Make sure to set a new GUID (or leave it empty if it's auto-generated)
            PodeCriarUtilizadores = permisssoDto.PodeCriarUtilizadores,
            PodeEditarUtilizadores = permisssoDto.PodeEditarUtilizadores,
            PodeVerRegistos = permisssoDto.PodeVerRegistos,
            PodeAlterarPermissoes = permisssoDto.PodeAlterarPermissoes
        };

        // Add to the database
        _context.Permissoes.Add(permissao);
        await _context.SaveChangesAsync();

        // Map the saved model back to a DTO to return to the client
        var createdPermissoDto = new PermissoDto
        {
            Id = permissao.Id,
            PodeCriarUtilizadores = permissao.PodeCriarUtilizadores,
            PodeEditarUtilizadores = permisssoDto.PodeEditarUtilizadores,
            PodeVerRegistos = permissao.PodeVerRegistos,
            PodeAlterarPermissoes = permissao.PodeAlterarPermissoes
        };

        // Return the created DTO with a 201 status
        return CreatedAtAction(nameof(GetPermisso), new { id = createdPermissoDto.Id }, createdPermissoDto);
    }

    // DELETE: api/Permisso/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePermisso(Guid id)
    {
        var permissao = await _context.Permissoes.FindAsync(id);
        if (permissao == null)
        {
            return NotFound();
        }

        _context.Permissoes.Remove(permissao);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PermissoExists(Guid id)
    {
        return _context.Permissoes.Any(e => e.Id == id);
    }
}

}

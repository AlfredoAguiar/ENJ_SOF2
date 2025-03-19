using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication5.DTO;  
public class PermissoService
{
    private readonly HttpClient _httpClient;

    public PermissoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PermissoDto>> GetAllPermissoesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<PermissoDto>>("https://localhost:5189/api/permissoes");
    }

    public async Task<PermissoDto> GetPermissoByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<PermissoDto>($"https://localhost:5189/api/permissoes/{id}");
    }

    public async Task<bool> CreatePermissoAsync(PermissoDto permissoDto)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:5189/api/permissoes", permissoDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdatePermissoAsync(Guid id, PermissoDto permissoDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:5189/api/permissoes/{id}", permissoDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePermissoAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:5189/api/permissoes/{id}");
        return response.IsSuccessStatusCode;
    }
}
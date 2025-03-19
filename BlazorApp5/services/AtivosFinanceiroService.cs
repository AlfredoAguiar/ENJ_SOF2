using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication5.DTO; 

public class AtivosFinanceiroService
{
    private readonly HttpClient _httpClient;

    public AtivosFinanceiroService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<AtivosFinanceiroDto>> GetAllAtivosFinanceirosAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<AtivosFinanceiroDto>>("https://localhost:5189/api/ativosfinanceiros");
    }

    public async Task<AtivosFinanceiroDto> GetAtivosFinanceiroByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<AtivosFinanceiroDto>($"https://localhost:5189/api/ativosfinanceiros/{id}");
    }

    public async Task<bool> CreateAtivosFinanceiroAsync(AtivosFinanceiroDto ativosFinanceiroDto)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:5189/api/ativosfinanceiros", ativosFinanceiroDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAtivosFinanceiroAsync(Guid id, AtivosFinanceiroDto ativosFinanceiroDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:5189/api/ativosfinanceiros/{id}", ativosFinanceiroDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAtivosFinanceiroAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:5189/api/ativosfinanceiros/{id}");
        return response.IsSuccessStatusCode;
    }
}
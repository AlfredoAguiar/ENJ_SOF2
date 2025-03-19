using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication5.DTO;  

public class DepositoService
{
    private readonly HttpClient _httpClient;

    public DepositoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<DepositoDto>> GetAllDepositosAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<DepositoDto>>("https://localhost:5189/api/depositos");
    }

    public async Task<DepositoDto> GetDepositoByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<DepositoDto>($"https://localhost:5189/api/depositos/{id}");
    }

    public async Task<bool> CreateDepositoAsync(DepositoDto depositoDto)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:5189/api/depositos", depositoDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateDepositoAsync(Guid id, DepositoDto depositoDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:5189/api/depositos/{id}", depositoDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteDepositoAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:5189/api/depositos/{id}");
        return response.IsSuccessStatusCode;
    }
}
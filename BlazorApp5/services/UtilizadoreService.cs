using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication5.DTO;  

public class UtilizadoreService
{
    private readonly HttpClient _httpClient;

    public UtilizadoreService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UtilizadoreDto>> GetAllUtilizadoresAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<UtilizadoreDto>>("https://localhost:5189/api/utilizadore");
    }

    public async Task<UtilizadoreDto> GetUtilizadoreByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<UtilizadoreDto>($"https://localhost:5189/api/utilizadore/{id}");
    }

    public async Task<bool> CreateUtilizadoreAsync(UtilizadoreDto utilizadoreDto)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:5189/api/utilizadore", utilizadoreDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateUtilizadoreAsync(Guid id, UtilizadoreDto utilizadoreDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:5189/api/utilizadore/{id}", utilizadoreDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteUtilizadoreAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:5189/api/utilizadore/{id}");
        return response.IsSuccessStatusCode;
    }
}
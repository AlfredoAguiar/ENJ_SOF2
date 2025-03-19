using System.Net.Http.Json;
using WebApplication5.DTO;

namespace BlazorApp1.services;

public class exemple_conect_to_API
{

    private readonly HttpClient _httpClient;

    public exemple_conect_to_API(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Obter lista de utilizadores
    public async Task<List<UtilizadoreDto>> GetUtilizadoresAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<UtilizadoreDto>>("https://localhost:5189/api/utilizadore");
    }

    // Obter um utilizador por ID
    public async Task<UtilizadoreDto> GetUtilizadorAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<UtilizadoreDto>($"https://localhost:5189/api/utilizadore/{id}");
    }

    // Adicionar um utilizador
    public async Task<bool> AddUtilizadorAsync(UtilizadoreDto utilizadorDto)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:5189/api/utilizadore", utilizadorDto);
        return response.IsSuccessStatusCode;
    }
}
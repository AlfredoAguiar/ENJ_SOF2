using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication5.DTO;  

public class LocalizacaoService
{
    private readonly HttpClient _httpClient;

    public LocalizacaoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<LocalizacoDto>> GetAllLocalizacoesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<LocalizacoDto>>("https://localhost:5189/api/localizacoes");
    }

    public async Task<LocalizacoDto> GetLocalizacaoByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<LocalizacoDto>($"https://localhost:5189/api/localizacoes/{id}");
    }

    public async Task<bool> CreateLocalizacaoAsync(LocalizacoDto localizacaoDto)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:5189/api/localizacoes", localizacaoDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateLocalizacaoAsync(Guid id, LocalizacoDto localizacaoDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:5189/api/localizacoes/{id}", localizacaoDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteLocalizacaoAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:5189/api/localizacoes/{id}");
        return response.IsSuccessStatusCode;
    }
}
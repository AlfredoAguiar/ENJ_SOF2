using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication5.DTO;  

public class ImoveisArrendamentoService
{
    private readonly HttpClient _httpClient;

    public ImoveisArrendamentoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Obter todos os Imóveis de Arrendamento
    public async Task<List<ImoveisArrendamentoDto>> GetAllImoveisArrendamentoAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ImoveisArrendamentoDto>>("https://localhost:5189/api/imoveisarrendamento");
    }

    // Obter Imóvel de Arrendamento por ID
    public async Task<ImoveisArrendamentoDto> GetImoveisArrendamentoByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<ImoveisArrendamentoDto>($"https://localhost:5189/api/imoveisarrendamento/{id}");
    }

    // Criar um novo Imóvel de Arrendamento
    public async Task<bool> CreateImoveisArrendamentoAsync(ImoveisArrendamentoDto imoveisArrendamentoDto)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:5189/api/imoveisarrendamento", imoveisArrendamentoDto);
        return response.IsSuccessStatusCode;
    }

    // Atualizar um Imóvel de Arrendamento
    public async Task<bool> UpdateImoveisArrendamentoAsync(Guid id, ImoveisArrendamentoDto imoveisArrendamentoDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:5189/api/imoveisarrendamento/{id}", imoveisArrendamentoDto);
        return response.IsSuccessStatusCode;
    }

    // Deletar um Imóvel de Arrendamento
    public async Task<bool> DeleteImoveisArrendamentoAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:5189/api/imoveisarrendamento/{id}");
        return response.IsSuccessStatusCode;
    }
}
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication5.DTO;  

public class FundoInvestimentoService
{
    private readonly HttpClient _httpClient;

    public FundoInvestimentoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<FundosInvestimentoDto>> GetAllFundosInvestimentoAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<FundosInvestimentoDto>>("https://localhost:5189/api/fundosinvestimento");
    }

    public async Task<FundosInvestimentoDto> GetFundoInvestimentoByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<FundosInvestimentoDto>($"https://localhost:5189/api/fundosinvestimento/{id}");
    }

    public async Task<bool> CreateFundoInvestimentoAsync(FundosInvestimentoDto fundoInvestimentoDto)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:5189/api/fundosinvestimento", fundoInvestimentoDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateFundoInvestimentoAsync(Guid id, FundosInvestimentoDto fundoInvestimentoDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:5189/api/fundosinvestimento/{id}", fundoInvestimentoDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteFundoInvestimentoAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:5189/api/fundosinvestimento/{id}");
        return response.IsSuccessStatusCode;
    }
}
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication5.DTO;  

public class CarteiraService
{
    private readonly HttpClient _httpClient;

    public CarteiraService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CarteiraDto>> GetAllCarteirasAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CarteiraDto>>("https://localhost:5189/api/carteiras");
    }

    public async Task<CarteiraDto> GetCarteiraByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<CarteiraDto>($"https://localhost:5189/api/carteiras/{id}");
    }

    public async Task<bool> CreateCarteiraAsync(CarteiraDto carteiraDto)
    {
        var response = await _httpClient.PostAsJsonAsync("https://localhost:5189/api/carteiras", carteiraDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCarteiraAsync(Guid id, CarteiraDto carteiraDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"https://localhost:5189/api/carteiras/{id}", carteiraDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCarteiraAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:5189/api/carteiras/{id}");
        return response.IsSuccessStatusCode;
    }
}
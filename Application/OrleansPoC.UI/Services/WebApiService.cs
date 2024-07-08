using System.Text.Json;
using OrleansPoC.UI.Models;

namespace OrleansPoC.UI.Services;

public class WebApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public WebApiService(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory;
    
    public async Task<Customer> CreateCustomerAsync(string name, string[] stocks)
    {
        var client = _httpClientFactory.CreateClient();
        var json = JsonSerializer.Serialize(new { name, stocks });
        var response = await client.PostAsJsonAsync("customers", json);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Customer>(content) ?? throw new Exception("Failed to create customer");
    }

    public async Task<Customer> GetCustomerAsync(Guid id)
    {
        var client = _httpClientFactory.CreateClient();
        var content = await client.GetStringAsync($"customers/{id}");
        return JsonSerializer.Deserialize<Models.Customer>(content) ?? throw new Exception("Failed to create customer");
    }

    public async Task GetStocksAsync(Guid id, Action<Stock> onReceiveStock)
    {
        var client = _httpClientFactory.CreateClient();
        var stream = await client.GetStreamAsync($"customers/{id}/stocks");

        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            var parts = line?.Split(':');
            if (parts is null || parts.Length != 3) continue;
            onReceiveStock(new Stock
            {
                Name = parts[1].Trim(),
                Value = decimal.Parse(parts[2].Trim())
            });
        }
    }
}
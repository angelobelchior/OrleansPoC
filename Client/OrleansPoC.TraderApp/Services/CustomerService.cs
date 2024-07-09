using System.Globalization;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using OrleansPoC.TraderApp.Models;

namespace OrleansPoC.TraderApp.Services;

public class CustomerService(string baseAddress) : IDisposable
{
    private readonly HttpClient _client = new() { BaseAddress = new Uri(baseAddress) };

    public async Task<Customer> CreateCustomerAsync(string name, string[] stocks)
    {
        var json = JsonSerializer.Serialize(new { name, stocks });
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("customers", httpContent);
        var content = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<Customer>(content) ?? throw new Exception("Failed to create customer");
    }

    public async Task<Customer> GetCustomerAsync(Guid id)
    {
        var content = await _client.GetStringAsync($"customers/{id}");
        return JsonSerializer.Deserialize<Customer>(content) ?? throw new Exception("Failed to create customer");
    }

    public async Task BeginReceivingStocksAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var stream = await _client.GetStreamAsync($"customers/{customerId}/stocks", cancellationToken);

        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
            if(cancellationToken.IsCancellationRequested) break;
            
            var line = await reader.ReadLineAsync(cancellationToken);
            var parts = line?.Split(':');
            if (parts is null || parts.Length != 3) continue;
            var stock = new Stock
            {
                Name = parts[1].Trim(),
                Value = decimal.Parse(parts[2].Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture)
            };
            WeakReferenceMessenger.Default.Send(new StockChangedMessage(stock));
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}

public class StockChangedMessage(Stock stock) : ValueChangedMessage<Stock>(stock);
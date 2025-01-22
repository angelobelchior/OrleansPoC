using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.ServerSentEvents;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using OrleansPoC.Contracts.Models;
using OrleansPoC.UI.Models;

namespace OrleansPoC.UI.ViewModels;

public class StocksViewModel : ObservableObject
{
    public required Customer Customer { get; init; }
    public required string Endpoint { get; init; }
    public ObservableCollection<StockValueItem> Stocks { get; set; } = [];

    public Func<Task>? OnInitializedAction { get; set; }
    public Func<Task>? OnFinalizedAction { get; set; }

    private readonly CancellationTokenSource _cancellationTokenSource;

    public StocksViewModel()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        OnInitializedAction = OnInitialized;
        OnFinalizedAction = OnFinalized;
    }

    private async Task OnInitialized()
    {
        try
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(Endpoint)
            };
            var url = $"customers/{Customer.Id}/stocks";
            await using var stream = await httpClient.GetStreamAsync(url, _cancellationTokenSource.Token).ConfigureAwait(true);
            await foreach (var item in SseParser.Create(stream).EnumerateAsync(_cancellationTokenSource.Token).ConfigureAwait(true))
            {
                if (item.EventType != "stockChanged") continue;
                
                var stock = JsonSerializer.Deserialize<StockValueItem>(item.Data);
                if(stock is null) continue;

                var index = Stocks.IndexOf(stock);
                if (index == -1)
                    Stocks.Add(stock);
                else
                    Stocks.ElementAt(index).Value = stock.Value;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task OnFinalized()
    {
        OnInitializedAction = null;
        OnInitializedAction = null;
        await _cancellationTokenSource.CancelAsync().ConfigureAwait(true);
    }
}
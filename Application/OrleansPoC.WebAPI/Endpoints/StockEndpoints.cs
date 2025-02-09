using System.Text.Json;
using OrleansPoC.Contracts.Grains;
using OrleansPoC.Contracts.Models;
using Observers = System.Collections.Generic.List<(OrleansPoC.Contracts.Grains.IStockGrain StockGrain, OrleansPoC.Contracts.Grains.IStockObserver ConsumerObserver)>;

namespace OrleansPoC.WebAPI.Endpoints;

public static class StocksEndpoints
{
    public static void MapCustomers(this WebApplication app)
    {
        app.MapGet("stocks/{stockName}/book", (
                IClusterClient clusterClient,
                string stockName,
                HttpContext context,
                CancellationToken cancellationToken)
            =>
        {
            var observers = new Observers();
            try
            {
                Subscribe(observers, clusterClient, stockName, context, cancellationToken);
            }
            catch
            {
                Unsubscribe(observers);
            }
            return Results.Empty;
        });
    }
    
    private static void Subscribe(
        Observers observers,
        IClusterClient clusterClient,
        string stockName,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        context.Response.Headers.Append("Content-Type", "text/event-stream");
        
        var stockGrain = clusterClient.GetGrain<IStockGrain>(stockName);

        var observer = new StockObserver(context.Response, cancellationToken);
        var observerRef = clusterClient.CreateObjectReference<IStockObserver>(observer);

        observers.Add((stockGrain, observerRef));

        stockGrain.Subscribe(observerRef);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            // Keep the connection open
        }
    }
    
    private static void Unsubscribe(Observers observers)
    {
        foreach (var (stocksGrain, consumerObserver) in observers)
        {
            try
            {
                stocksGrain.Unsubscribe(consumerObserver);
            }
            catch
            {
                // ignored
            }
            
        }
    }

    private class StockObserver(HttpResponse response, CancellationToken cancellationToken) : IStockObserver
    {
        public async Task OnStockUpdated(Stock stock)
        {
            await response.WriteAsync("event: stockChanged", cancellationToken: cancellationToken);
            await response.WriteAsync("\n", cancellationToken: cancellationToken);
            await response.WriteAsync("data: ", cancellationToken: cancellationToken);
            await JsonSerializer.SerializeAsync(response.Body, stock, cancellationToken: cancellationToken);
            await response.WriteAsync("\n\n", cancellationToken: cancellationToken);
            await response.Body.FlushAsync(cancellationToken);
        }
    }
}
using OrleansPoC.Contracts.Grains;
using OrleansPoC.Contracts.Models;

namespace OrleansPoC.Worker.MarketData;

public class Worker(IClusterClient client)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var stock = CreateStock();
                var grain = client.GetGrain<IStockGrain>(stock.Name);
                await grain.Send(stock.Value);
                await Task.Delay(Random.Shared.Next(50), stoppingToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private static Stock CreateStock()
    {
        var stocks = Stock.GetStockList();
        return new Stock
        {
            Name = stocks[Random.Shared.Next(stocks.Length)],
            Value = (decimal)(Random.Shared.NextDouble() * 100)
        };
    }
}
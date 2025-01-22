using OrleansPoC.Contracts.Grains;
using OrleansPoC.Contracts.Models;

namespace OrleansPoC.Server.Grains;

public class StockGrain(
    ILogger<StockGrain> logger)
    : Grain, IStockGrain
{
    private readonly HashSet<IConsumerObserver> _observers = new();

    public async ValueTask Send(decimal value)
    {
        var stock = new Stock
        {
            Name = this.GetPrimaryKeyString(),
            Value = value
        };

        foreach (var consumerObserver in _observers)
            await consumerObserver.OnStockUpdated(stock);
    }

    public void Subscribe(IConsumerObserver observer)
        => _observers.Add(observer);

    public void Unsubscribe(IConsumerObserver observer)
        => _observers.Remove(observer);
}
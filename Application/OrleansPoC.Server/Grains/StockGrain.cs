using OrleansPoC.Contracts.Grains;
using OrleansPoC.Contracts.Models;

namespace OrleansPoC.Server.Grains;

public class StockGrain
    : Grain, IStockGrain
{
    private readonly HashSet<IStockObserver> _observers = new();

    public async ValueTask Update(Stock stock)
    {
        foreach (var observer in _observers)
        {
            try
            {
                await observer.OnStockUpdated(stock);
            }
            catch
            {
                // ignored
            }
        }
    }

    public void Subscribe(IStockObserver observer)
        => _observers.Add(observer);

    public void Unsubscribe(IStockObserver observer)
        => _observers.Remove(observer);

    public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        if (reason.ReasonCode == DeactivationReasonCode.ShuttingDown)
            MigrateOnIdle();

        return base.OnDeactivateAsync(reason, cancellationToken);
    }
}
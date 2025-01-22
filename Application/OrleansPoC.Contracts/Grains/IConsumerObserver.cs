using OrleansPoC.Contracts.Models;

namespace OrleansPoC.Contracts.Grains;

[Alias("OrleansPoC.Contracts.Grains.IConsumerObserver")]
public interface IConsumerObserver : IGrainObserver
{
    [Alias("OnStockUpdated")]
    Task OnStockUpdated(Stock stock);
}
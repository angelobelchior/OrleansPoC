using OrleansPoC.Contracts.Models;

namespace OrleansPoC.Contracts.Grains;

[Alias("OrleansPoC.Contracts.Grains.IStockObserver")]
public interface IStockObserver : IGrainObserver
{
    [Alias("OnStockUpdated")]
    Task OnStockUpdated(Stock stock);
}
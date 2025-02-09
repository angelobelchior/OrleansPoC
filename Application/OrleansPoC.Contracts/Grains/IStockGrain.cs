using OrleansPoC.Contracts.Models;

namespace OrleansPoC.Contracts.Grains;

[Alias("OrleansPoC.Contracts.Grains.IStockUpdatedGrain")]
public interface IStockGrain : IGrainWithStringKey
{
    [Alias("Send")]
    ValueTask Update(Stock stock);
    
    [Alias("Subscribe")]
    void Subscribe(IStockObserver observer);
    
    [Alias("Unsubscribe")]
    void Unsubscribe(IStockObserver observer);
}
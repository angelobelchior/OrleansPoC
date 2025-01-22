namespace OrleansPoC.Contracts.Grains;

[Alias("OrleansPoC.Contracts.Grains.IStockUpdatedGrain")]
public interface IStockGrain : IGrainWithStringKey
{
    [Alias("Send")]
    ValueTask Send(decimal value);
    
    [Alias("Subscribe")]
    void Subscribe(IConsumerObserver observer);
    
    [Alias("Unsubscribe")]
    void Unsubscribe(IConsumerObserver observer);
}
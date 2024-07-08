using OrleansPoC.Contracts.Models;

namespace OrleansPoC.Contracts.Grains;

[Alias("OrleansPoC.Contracts.Grains.IStockUpdatedGrain")]
public interface IStockGrain : IGrainWithStringKey
{
    [Alias("Send")]
    ValueTask Send(decimal value);
    
    [Alias("Get")]
    Task<Stock> Get();
}
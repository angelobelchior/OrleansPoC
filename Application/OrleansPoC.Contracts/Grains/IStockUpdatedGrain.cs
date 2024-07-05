namespace OrleansPoC.Contracts.Grains;

[Alias("OrleansPoC.Contracts.Grains.IStockUpdatedGrain")]
public interface IStockUpdatedGrain : IGrainWithStringKey
{
    [Alias("Send")]
    ValueTask Send(decimal value);
}
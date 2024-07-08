using System.Collections.Concurrent;
using Orleans.Runtime;
using OrleansPoC.Contracts.Grains;
using OrleansPoC.Contracts.Models;

namespace OrleansPoC.Server.Grains;

public class StockGrain(
    ILogger<StockGrain> logger,
    [PersistentState(
        stateName: "stocks",
        storageName: "stocks")]
    IPersistentState<Stock> persistent) 
    : Grain, IStockGrain
{
    public async ValueTask Send(decimal value)
    {
        var stock = new Stock
        {
            Name = this.GetPrimaryKeyString(),
            Value = value
        };
        logger.LogInformation("Server.Send[{Datetime}] => {Stock}:{Value}", DateTime.Now, stock.Name, stock.Value);
        persistent.State = stock;
        await persistent.WriteStateAsync();
    }
    
    public Task<Stock> Get()
    {
        logger.LogInformation("Server.Get[{Datetime}] => {Stock}", DateTime.Now, persistent.State.Name);
        return Task.FromResult(persistent.State);
    }
}
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
    
    public Task<Stock[]> Get(string name)
    {
        //var stocks = persistent.State.Where(s => s.Name == name).ToArray();
        Stock[] stocks = [persistent.State];
        logger.LogInformation("Server.Get[{Datetime}] => {Stock}", DateTime.Now, name);
        return Task.FromResult(stocks);
    }
}
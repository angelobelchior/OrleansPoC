using Orleans.Runtime;
using OrleansPoC.Contracts.Grains;
using OrleansPoC.Contracts.Models;

namespace OrleansPoC.Server.Grains;

public class CustomerGrain(
    ILogger<StockGrain> logger,
    [PersistentState(
        stateName: "customers",
        storageName: "customers")]
    IPersistentState<Customer> persistent)
    : Grain, ICustomerGrain
{
    public async Task<Customer> Create(string name, string[] stockNames)
    {
        var customer = new Customer
        {
            Id = this.GetPrimaryKey(),
            Name = name,
            Stocks = stockNames.Select(s => new Stock { Name = s }).ToArray()
        };
        persistent.State = customer;
        await persistent.WriteStateAsync();
        return customer;
    }

    public Task<Customer> Get()
    {
        return Task.FromResult(persistent.State);
    }

    public async Task<Customer> UpdateStock(string name, decimal value)
    {
        var customer = await Get();
        customer.Stocks.First(s => s.Name == name).Value = value;
        persistent.State = customer;
        await persistent.WriteStateAsync();
        return customer;
    }
}
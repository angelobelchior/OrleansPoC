using OrleansPoC.Contracts.Grains;
using OrleansPoC.Contracts.Models;

namespace OrleansPoC.Server.Grains;

public class CustomerGrain(
    ILogger<StockGrain> logger,
    [PersistentState(
        stateName: "customers",
        storageName: "customers")]
    IPersistentState<Customer?> persistent)
    : Grain, ICustomerGrain
{
    public async Task<Customer> Create(string name, string[] stockNames)
    {
        var customer = new Customer
        {
            Id = this.GetPrimaryKey(),
            Name = name,
            Stocks = stockNames
        };
        persistent.State = customer;
        await persistent.WriteStateAsync();
        return customer;
    }

    public Task<Customer?> Get()
        => Task.FromResult(persistent.State);
}
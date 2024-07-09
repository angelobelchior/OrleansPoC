using OrleansPoC.Contracts.Models;

namespace OrleansPoC.Contracts.Grains;

[Alias("OrleansPoC.Contracts.Grains.ICustomerGrain")]
public interface ICustomerGrain : IGrainWithGuidKey
{
    [Alias("Create")]
    Task<Customer> Create(string name, string[] stockNames);

    [Alias("Get")]
    Task<Customer> Get();
    
    [Alias("Update")]
    Task<Customer> Update(string name, string[] stockNames);
}
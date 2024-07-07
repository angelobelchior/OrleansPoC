namespace OrleansPoC.Contracts.Models;

[GenerateSerializer, Alias("OrleansPoC.Contracts.Models.Customer")]
public class Customer
{
    [Id(0)]
    public Guid Id { get; set; }
    
    [Id(1)]
    public string Name { get; set; } = string.Empty;
    
    [Id(2)]
    public Stock[] Stocks { get; set; } = [];
}
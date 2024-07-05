namespace OrleansPoC.Contracts.Models;

[GenerateSerializer, Alias("OrleansPoC.Contracts.Models.Stock")]
public class Stock
{
    public static readonly Stock Empty = new Stock { Name = string.Empty, Value = 0 };
    
    [Id(0)]
    public string Name { get; set; } = string.Empty;
    [Id(1)]
    public decimal Value { get; set; }
}
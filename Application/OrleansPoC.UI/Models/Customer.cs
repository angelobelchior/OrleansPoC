namespace OrleansPoC.UI.Models;

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string[] Stocks { get; set; } = [];
}
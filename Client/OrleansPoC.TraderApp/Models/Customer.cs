using System.Text.Json.Serialization;

namespace OrleansPoC.TraderApp.Models;

public class Customer
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("stocks")]
    public string[] Stocks { get; set; } = [];
}
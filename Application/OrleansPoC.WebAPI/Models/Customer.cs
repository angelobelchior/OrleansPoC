using System.ComponentModel.DataAnnotations;

namespace OrleansPoC.WebAPI.Models;

public class Customer
{
    [Required, MinLength(3)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string[] Stocks { get; set; } = [];
}
using CommunityToolkit.Mvvm.ComponentModel;

namespace OrleansPoC.TraderApp.Models;

public partial class Stock : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;
    
    [ObservableProperty]
    private decimal _value;
    
    [ObservableProperty]
    private string _variationIcon = string.Empty;
}
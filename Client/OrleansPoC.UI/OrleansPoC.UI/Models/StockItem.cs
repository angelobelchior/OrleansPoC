namespace OrleansPoC.UI.Models;

public partial class StockItem : ObservableObject
{
    [ObservableProperty] public string _name = string.Empty;
    [ObservableProperty] public bool _isChecked;
}
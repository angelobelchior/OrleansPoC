using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OrleansPoC.UI.Models;

public partial class StockValueItem : ObservableObject, IEquatable<StockValueItem>
{
    [ObservableProperty] public string _name = string.Empty;
    [ObservableProperty] public decimal _value;

    public bool Equals(StockValueItem? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((StockValueItem)obj);
    }
}
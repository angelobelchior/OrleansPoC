using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace OrleansPoC.UI.Views.Converters;

public class NegativeNumberConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var number = (decimal)value!;
        return number > 0 ? Brushes.Blue : Brushes.Brown;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => 0;
}
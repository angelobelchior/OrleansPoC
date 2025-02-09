using System.Globalization;
using Avalonia.Data.Converters;

namespace OrleansPoC.UI.Views.Converters;

public class SizeBarConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var number = (int)value!;
        if (number == 0) number = 1;
        var barWidth = int.Parse(parameter!.ToString()!);
        return (barWidth / 100) * (number * 10);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => 0;
}
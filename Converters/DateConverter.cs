using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Humanizer;

namespace Skimmer.Avalonia.Converters;

public class DateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is DateTime date ? date.Humanize() : (object?)null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
using System;
using System.Globalization;

using Avalonia.Data.Converters;
// ReSharper disable ReturnTypeCanBeNotNullable
// Simply because the Interface requires it
namespace Skimmer.Avalonia.Converters;
internal class StringToHtmlLinkConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return $"<a href=\"{value}\">{value}</a>";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

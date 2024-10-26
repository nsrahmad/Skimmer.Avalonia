using System;
using System.Globalization;

using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
// ReSharper disable ReturnTypeCanBeNotNullable

namespace Skimmer.Avalonia.Converters;

public class ThemeColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Application.Current!.TryGetResource("TextControlForeground", Application.Current.ActualThemeVariant, out var fg);
        Application.Current.TryGetResource("TextControlBackground", Application.Current.ActualThemeVariant, out var bg);
        Application.Current.TryGetResource("ControlContentThemeFontSize", Application.Current.ActualThemeVariant, out var size);

        var fgBrush = (SolidColorBrush)fg!;
        var bgBrush = (SolidColorBrush)bg!;
        
        // ReSharper disable once StringLiteralTypo
        return $"""
                <div style="color : rgba({fgBrush.Color.R},{fgBrush.Color.G},{fgBrush.Color.B},{fgBrush.Color.A});
                    font-size: {size}px; 
                    font-family: Inter,Segoe UI,Frutiger,Frutiger Linotype,Dejavu Sans,Helvetica Neue,Arial,sans-serif;
                    background-color: rgba({bgBrush.Color.R},{bgBrush.Color.G},{bgBrush.Color.B},{bgBrush.Color.A});">
                {value}
                </div>
                """;
    }
    
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return new NotSupportedException();
    }
}
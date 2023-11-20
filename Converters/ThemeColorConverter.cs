using System;
using System.Globalization;

using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Skimmer.Avalonia.Converters;

public class ThemeColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Application.Current!.TryGetResource("TextControlForeground", Application.Current.ActualThemeVariant, out var fg);
        Application.Current!.TryGetResource("TextControlBackground", Application.Current.ActualThemeVariant, out var bg);
        Application.Current!.TryGetResource("ControlContentThemeFontSize", Application.Current.ActualThemeVariant, out var size);

        var fgBrush = (SolidColorBrush)fg!;
        var bgBrush = (SolidColorBrush)bg!;
        var fontSize = (double)size!;
        
        /* For a more complete Web renderer, this is best. But not supported by Avalonia.Html
         *  <style> 
                  html * { 
                    color : rgba({{fgBrush.Color.R}},{{fgBrush.Color.G}},{{fgBrush.Color.B}},{{fgBrush.Color.A}});
                    background-color : rgba({{bgBrush.Color.R}},{{bgBrush.Color.G}},{{bgBrush.Color.B}},{{bgBrush.Color.A}});
                    font-family: Inter,Segoe UI,Frutiger,Frutiger Linotype,Dejavu Sans,Helvetica Neue,Arial,sans-serif; 
                    font-size: {{fontSize}}px;
                  }
     
                </style> 
         */

        var text = $$"""
                <body style=" color : rgba({{fgBrush.Color.R}},{{fgBrush.Color.G}},{{fgBrush.Color.B}},{{fgBrush.Color.A}});">
                {{value}}
                </body>
                """;
        return text;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}
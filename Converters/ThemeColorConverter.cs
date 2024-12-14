// Copyright © Nisar Ahmad
// 
// This program is free software:you can redistribute it and/or modify it under the terms of
// the GNU General Public License as published by the Free Software Foundation, either
// version 3 of the License, or (at your option) any later version.
// This program is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY, without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE.See the GNU General Public License for more details.
// You should have received a copy of the GNU General Public License along with this
// program.If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Skimmer.Avalonia.Converters;

public class ThemeColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Application.Current!.TryGetResource("TextControlForeground", Application.Current.ActualThemeVariant,
            out object? fg);
        Application.Current.TryGetResource("TextControlBackground", Application.Current.ActualThemeVariant,
            out object? bg);
        Application.Current.TryGetResource("ControlContentThemeFontSize", Application.Current.ActualThemeVariant,
            out object? size);

        SolidColorBrush fgBrush = (SolidColorBrush)fg!;
        SolidColorBrush bgBrush = (SolidColorBrush)bg!;

        return $"""
                <div style="color : rgba({fgBrush.Color.R},{fgBrush.Color.G},{fgBrush.Color.B},{fgBrush.Color.A});
                    font-size: {size}px;
                    font-family: Inter,Segoe UI,Frutiger,Frutiger Linotype,Dejavu Sans,Helvetica Neue,Arial,sans-serif;
                    background-color: rgba({bgBrush.Color.R},{bgBrush.Color.G},{bgBrush.Color.B},{bgBrush.Color.A});">
                {value}
                </div>
                """;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        new NotSupportedException();
}

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
        Application.Current!.TryGetResource("TextFillColorPrimary", Application.Current.ActualThemeVariant,
            out object? fg);
        Application.Current.TryGetResource("CardBackgroundFillColorDefault", Application.Current.ActualThemeVariant,
            out object? bg);
        Application.Current.TryGetResource("ControlContentThemeFontSize", Application.Current.ActualThemeVariant,
            out object? size);

        Color foreground = (Color)fg!;
        Color background = (Color)bg!;

        return $"""
                <div style="color : rgba({foreground.R},{foreground.G},{foreground.B},{foreground.A});
                    font-size: {size}px;
                    padding: {size}px;
                    font-family: Segoe UI,Inter,Frutiger,Frutiger Linotype,Dejavu Sans,Helvetica Neue,Arial,sans-serif;
                    background-color: rgba({background.R},{background.G},{background.B},{background.A});">
                {value}
                </div>
                """;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        new NotSupportedException();
}

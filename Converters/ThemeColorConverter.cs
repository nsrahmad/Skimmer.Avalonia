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
using ActiproSoftware.UI.Avalonia.Themes;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace Skimmer.Avalonia.Converters;

public class ThemeColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        Application.Current!.TryGetResource(ThemeResourceKind.DefaultForegroundBrush.ToResourceKey(),
            Application.Current.ActualThemeVariant,
            out object? fg);
        Application.Current.TryGetResource(ThemeResourceKind.DefaultBackgroundBrush.ToResourceKey(),
            Application.Current.ActualThemeVariant,
            out object? bg);
        Application.Current.TryGetResource(ThemeResourceKind.DefaultFontFamily.ToResourceKey(),
            Application.Current.ActualThemeVariant,
            out object? fm);
        Application.Current.TryGetResource(ThemeResourceKind.DefaultFontSizeMedium.ToResourceKey(),
            Application.Current.ActualThemeVariant,
            out object? size);
        Application.Current.TryGetResource(ThemeResourceKind.HyperlinkForegroundBrush.ToResourceKey(),
            Application.Current.ActualThemeVariant,
            out object? linkColor);

        ImmutableSolidColorBrush foreground = (ImmutableSolidColorBrush)fg!;
        SolidColorBrush background = (SolidColorBrush)bg!;
        ImmutableSolidColorBrush l = (ImmutableSolidColorBrush)linkColor!;

        return $$"""
                 <style>
                 a {
                   color : rgba({{l.Color.R}}, {{l.Color.G}}, {{l.Color.B}}, {{l.Color.A}});
                 }
                 </style>
                 <div style="color : rgba({{foreground.Color.R}},{{foreground.Color.G}},{{foreground.Color.B}},{{foreground.Color.A}});
                      font-size: {{size}}px;
                      font-family: {{((FontFamily)fm!).Name}}, sans-serif;
                      background-color: rgba({{background.Color.R}},{{background.Color.G}},{{background.Color.B}},{{background.Color.A}});">
                      {{value}}
                 </div>
                 """;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        new NotSupportedException();
}

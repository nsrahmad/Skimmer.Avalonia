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
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Skimmer.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    private void HyperlinkTextBlock_OnClick(object? sender, RoutedEventArgs e) =>
        GetTopLevel(UrlTextBlock)!.Launcher.LaunchUriAsync(new Uri(UrlTextBlock.Text!));
}

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
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Skimmer.Avalonia.ViewModels;
using Skimmer.Avalonia.Views;
using Skimmer.Core.Nanorm;
using Skimmer.Core.ViewModels;

namespace Skimmer.Avalonia;

public class App : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        try
        {
            NanormFeedManager manager = new();
            Task.Run(() => manager.InitDbAsync()).GetAwaiter().GetResult();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                //BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel(manager) };
            }

            base.OnFrameworkInitializationCompleted();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.InnerException != null ? e.InnerException.Message : e.Message);
        }
    }
}

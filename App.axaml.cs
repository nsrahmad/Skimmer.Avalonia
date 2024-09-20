using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Skimmer.Avalonia.ViewModels;
using Skimmer.Avalonia.Views;

namespace Skimmer.Avalonia;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // DI root
        var collection = new ServiceCollection();
        collection.AddTransient<MainWindowViewModel>();

        var services = collection.BuildServiceProvider();
        var vm = services.GetRequiredService<MainWindowViewModel>();
        vm.SeedDataCommand.ExecuteAsync(null);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
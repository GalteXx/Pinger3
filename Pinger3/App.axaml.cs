using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Pinger3.Services;
using Pinger3.Services.DependencyInjection;
using Pinger3.ViewModels.PageViewModels;
using Pinger3.Views;
using System.Linq;
using System.Threading.Tasks;

namespace Pinger3
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();

                var collection = new ServiceCollection();
                collection.AddCommonServices();
                var services = collection.BuildServiceProvider();

                _ = StartAsync(desktop, services);
            }

            base.OnFrameworkInitializationCompleted();
        }

        private async Task StartAsync(IClassicDesktopStyleApplicationLifetime desktop, ServiceProvider services)
        {
            var vmBuilder = services.GetRequiredService<ParsedTargetsViewModelsBuilder>();

            _ = services.GetRequiredService<ISettingsViewModel>(); //To properly subscribe to events, Maybe a little hacky?

            await vmBuilder.ParseConfigAndCreateViewModelsAsync();

            var vm = services.GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = new MainWindow()
            {
                DataContext = vm,
            };
            desktop.MainWindow.Show();
        }
        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Pinger3.Services;
using Pinger3.Views;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Services.Migration;
using Pinger3.Services.Pinging;

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
                DisableAvaloniaDataAnnotationValidation();
                desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnExplicitShutdown;

                var collection = new ServiceCollection();
                collection.AddCommonServices();
                var services = collection.BuildServiceProvider();

                var startTask = StartAsync(services);
                startTask.ContinueWith(t =>
                {
                    if (t.IsCanceled)
                    {
                        desktop.Shutdown();
                    }
                    else if (t.IsFaulted)
                    {
                        throw new Exception($"Unhandled exception in StartAsync: {t.Exception?.Flatten()}");
                    }
                }, TaskScheduler.Default);
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static async Task StartAsync(ServiceProvider services)
        {
            var migrator = services.GetRequiredService<PersistentDataMigrator>();
            await migrator.MigrateAsync();
            
            // TODO: Loading screen with closing option
            
            var cacheLoader = services.GetRequiredService<ICacheLoader>();
            await cacheLoader.LoadApplicationCacheAsync(CancellationToken.None);
            var scheduleLoader = services.GetRequiredService<ScheduleLoader>();
            await scheduleLoader.LoadScheduleAsync(CancellationToken.None);
            
            services.GetRequiredService<IWindowService>().ShowWindow<MainWindow>();
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
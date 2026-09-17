using Microsoft.Extensions.DependencyInjection;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using Pinger3.Services;
using Pinger3.Views;

namespace Pinger3
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<IAddressesStorageParser, XmlAddressStorageParser>();
            collection.AddSingleton<ParsedTargetsViewModelsBuilder>();
            collection.AddSingleton<IWindowService, WindowService>();
            collection.AddSingleton<ResponseAwaitingTimeUpdaterService, ResponseAwaitingTimeUpdaterService>();

            collection.AddSingleton<TrayIconService>();
            
            AddStorageParsingServices(collection);
            AddViewModels(collection);
            AddWindows(collection);
        }

        private static void AddStorageParsingServices(IServiceCollection collection)
        {
            collection.AddTransient<IAddressesStorageParser, XmlAddressStorageParser>();
            collection.AddTransient<IStorageReader, XmlStorageReader>();
            collection.AddTransient<EndpointModelFactory>();
        }

        private static void AddViewModels(IServiceCollection collection)
        {
            collection.AddSingleton<ISettingsViewModel, SettingsViewModel>();
            collection.AddSingleton<INavigationViewModel, NavigationViewModel>();
            collection.AddSingleton<IPopoutWindowViewModel, PopoutWindowViewModel>();
            collection.AddTransient<IMainWindowViewModel, MainWindowViewModel>();
        }

        private static void AddWindows(IServiceCollection collection)
        {
            collection.AddSingleton(sp => new PopoutWindow(sp.GetRequiredService<IPopoutWindowViewModel>()));
            collection.AddTransient(sp => new SettingsWindow(sp.GetRequiredService<ISettingsViewModel>()));
            collection.AddTransient(sp => new MainWindow(sp.GetRequiredService<IMainWindowViewModel>()));
        }
    }
}
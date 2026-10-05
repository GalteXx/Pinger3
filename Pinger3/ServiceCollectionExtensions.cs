using Microsoft.Extensions.DependencyInjection;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using Pinger3.Services;
using Pinger3.Services.Migration;
using Pinger3.Services.Pinging;
using Pinger3.Services.PopOut;
using Pinger3.Services.PopOut.Factories;
using Pinger3.ViewModels.Controls.Factories;
using Pinger3.Views;

namespace Pinger3
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<IWindowService, WindowService>();
            collection.AddSingleton<TrayIconService>();
            
            AddStorageServices(collection);
            AddPingServices(collection);
            AddPopOutServices(collection);
            
            AddViewModels(collection);
            AddWindows(collection);
        }

        private static void AddPopOutServices(IServiceCollection collection)
        {
            collection.AddSingleton<IPopOutCatalog, PopOutCatalog>();
        }

        private static void AddStorageServices(IServiceCollection collection)
        {
            collection.AddTransient<PersistentDataMigrator>();
            collection.AddTransient<IAddressesStorage, XmlAddressStorage>();
            collection.AddTransient<IStorageGateway, XmlStorageGateway>();
            collection.AddSingleton<IEndpointRepository, EndpointRepository>();
            collection.AddTransient<EndpointModelFactory>();
        }

        private static void AddPingServices(IServiceCollection collection)
        {
            collection.AddTransient<IPingTransport, PingTransport>();
            collection.AddSingleton<IPingManager, PingManager>();
            collection.AddTransient<IPingScheduler, PingScheduler>();
            collection.AddTransient<AddressResolver>();
            collection.AddTransient<EndpointRuntimeFactory>();
            collection.AddTransient<EndpointUpdateResolver>();
        }

        private static void AddViewModels(IServiceCollection collection)
        {
            collection.AddTransient<IPingTargetViewModelFactory, PingTargetViewModelFactory>();
            collection.AddTransient<PopOutEndpointViewModelFactory>();
            
            collection.AddSingleton<ISettingsViewModel, SettingsViewModel>();
            collection.AddSingleton<INavigationViewModel, NavigationViewModel>();
            collection.AddTransient<IPopoutWindowViewModel, PopoutWindowViewModel>();
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
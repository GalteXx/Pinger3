using Microsoft.Extensions.DependencyInjection;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using Pinger3.Views;
using System;

namespace Pinger3.Services.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<IAddressesConfigParser, AddressesConfigParser>();
            collection.AddSingleton<ParsedTargetsViewModelsBuilder>();
            collection.AddSingleton<IWindowService, WindowService>();
            collection.AddSingleton<INavigationViewModel, NavigationViewModel>();
            collection.AddSingleton<ResponseAwaitingTimeUpdaterService, ResponseAwaitingTimeUpdaterService>();
            collection.AddSingleton<IPopoutWindowViewModel, PopoutWindowViewModel>();
            collection.AddSingleton<ISettingsViewModel, SettingsViewModel>();



            collection.AddTransient(sp => { return new PopoutWindow(sp.GetRequiredService<IPopoutWindowViewModel>()); });
            collection.AddTransient( sp => { return new SettingsWindow(sp.GetRequiredService<ISettingsViewModel>()); });
            collection.AddTransient<MainWindowViewModel>();
        }
    }
}
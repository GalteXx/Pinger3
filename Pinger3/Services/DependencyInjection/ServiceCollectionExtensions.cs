using Avalonia.Platform;
using Microsoft.Extensions.DependencyInjection;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;
using Pinger3.Views;

namespace Pinger3.Services.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<IAddressesConfigParser, AddressesConfigParser>();
            collection.AddSingleton<ParsedTargetsViewModelsBuilder>();
            collection.AddSingleton<IWindowService, WindowService>();
            collection.AddSingleton<ResponseAwaitingTimeUpdaterService, ResponseAwaitingTimeUpdaterService>();

            collection.AddSingleton<ISettingsViewModel, SettingsViewModel>();
            collection.AddSingleton<INavigationViewModel, NavigationViewModel>();
            collection.AddSingleton<IPopoutWindowViewModel, PopoutWindowViewModel>();
            collection.AddTransient<IMainWindowViewModel, MainWindowViewModel>();

            collection.AddSingleton<TrayIconService>();

            collection.AddSingleton(sp => { return new PopoutWindow(sp.GetRequiredService<IPopoutWindowViewModel>()); });
            collection.AddTransient(sp => { return new SettingsWindow(sp.GetRequiredService<ISettingsViewModel>()); });
            collection.AddTransient(sp => { return new MainWindow(sp.GetRequiredService<IMainWindowViewModel>()); });
        }
}
}
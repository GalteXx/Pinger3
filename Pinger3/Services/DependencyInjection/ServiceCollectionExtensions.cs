using Microsoft.Extensions.DependencyInjection;
using Pinger3.ViewModels.Controls;
using Pinger3.ViewModels.PageViewModels;

namespace Pinger3.Services.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddSingleton<IWindowService, WindowService>();
            collection.AddSingleton<INavigationViewModel, NavigationViewModel>();
            collection.AddSingleton<IAddressesConfigParser, AddressesConfigParser>();
            collection.AddSingleton<ResponeAwaitingTimeUpdaterService, ResponeAwaitingTimeUpdaterService>();
            collection.AddScoped<MainWindowViewModel, MainWindowViewModel>();
        }
    }
}
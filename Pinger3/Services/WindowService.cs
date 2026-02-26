using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Pinger3.Services
{
    public class WindowService : IWindowService
    {
        private readonly IServiceProvider _serviceProvider;

        public WindowService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public void ShowWindow<T>() where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            window.Show();
        }
    }
}
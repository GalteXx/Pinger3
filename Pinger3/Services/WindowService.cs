using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Pinger3.Services
{
    public class WindowService(IServiceProvider serviceProvider) : IWindowService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public void ShowWindow<T>() where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            window.Show();
        }
    }
}
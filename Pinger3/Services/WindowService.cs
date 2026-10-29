using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Pinger3.Services;

public class WindowService(IServiceProvider serviceProvider) : IWindowService
{
    public void ShowWindow<T>() where T : Window
    {
        var scope =  serviceProvider.CreateScope();
        var window = scope.ServiceProvider.GetRequiredService<T>();
        
        window.Closed += (s, e) => scope.Dispose();
        window.Show();
    }
}
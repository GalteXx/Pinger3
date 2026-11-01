using System;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Pinger3.Models;
using Pinger3.ViewModels.PageViewModels;
using Pinger3.Views;

namespace Pinger3.Services;

public class WindowService(IServiceProvider serviceProvider) : IWindowService
{
    public void ShowWindow<T>() where T : Window
    {
        var scope = serviceProvider.CreateScope();
        var window = scope.ServiceProvider.GetRequiredService<T>();

        window.Closed += (s, e) => scope.Dispose();
        window.Show();
    }

    public void ShowEndpointEditorWindow(EndpointModel endpointModel)
    {
        var scope = serviceProvider.CreateScope();
        scope.ServiceProvider.GetRequiredService<IEndpointEditorViewModel>().SetEndpointModel(endpointModel);

        var window = scope.ServiceProvider.GetRequiredService<EndpointEditorWindow>();
        window.Closed += (_, _) => scope.Dispose();
        window.Show();
    }
}
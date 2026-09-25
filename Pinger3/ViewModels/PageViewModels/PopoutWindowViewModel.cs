using System;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Pinger3.Models;
using Pinger3.Services.PopOut;
using Pinger3.Services.PopOut.Factories;

namespace Pinger3.ViewModels.PageViewModels;

public partial class PopoutWindowViewModel : ObservableObject, IPopoutWindowViewModel
{
    private readonly PopOutEndpointViewModelFactory _factory;

    //TODO: BindStyles
    [ObservableProperty] private bool _clickThrough;
    [ObservableProperty] private string _selectedStyleClassName = "CompactResponseTime";
    [ObservableProperty] private double _opacity = 1;


    public PopoutWindowViewModel(IPopOutCatalog popOutCatalog, PopOutEndpointViewModelFactory factory)
    {
        _factory = factory;

        popOutCatalog.EndpointAdded += OnEndpointAdded;
        popOutCatalog.EndpointRemoved +=
            (_, id) => PingViewModels.Remove(PingViewModels.FirstOrDefault(p => p.Id == id)!);

        popOutCatalog.EndpointUpdated +=
            (_, e) => GetValueOrDefault(e.Id)?.UpdateFromModel(e);

        popOutCatalog.PingReceived +=
            (_, update) => GetValueOrDefault(update.Id)?.OnPingReceived(update);
        popOutCatalog.PingSent += (_, id) => GetValueOrDefault(id)?.OnPingSent();
    }

    private IPopOutEndpointViewModel? GetValueOrDefault(string id)
    {
        return PingViewModels.FirstOrDefault(vm => vm.Id == id);
    }

    private void OnEndpointAdded(object? sender, EndpointModel e)
    {
        PingViewModels.Add(_factory.Create(e));
    }

    public ObservableCollection<IPopOutEndpointViewModel> PingViewModels { get; } = [];

    partial void OnOpacityChanging(double value)
    {
        if (value is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(value), $"{value} is not in [0;1] range");
    }
}
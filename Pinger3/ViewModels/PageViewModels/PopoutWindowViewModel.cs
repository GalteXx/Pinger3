using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Pinger3.Services;
using Pinger3.Services.Pinging;
using Pinger3.Services.PopOut;
using Pinger3.Services.PopOut.Factories;
using Pinger3.ViewModels.Controls;

namespace Pinger3.ViewModels.PageViewModels;

public sealed partial class PopoutWindowViewModel : ObservableObject, IPopoutWindowViewModel, IDisposable
{
    private readonly List<IDisposable> _subscriptions;

    //TODO: BindStyles
    [ObservableProperty] private bool _clickThrough;
    [ObservableProperty] private double _opacity = 1;
    [ObservableProperty] private string _selectedStyleClassName = "CompactResponseTime";


    public PopoutWindowViewModel(IPopOutManager popOutManager, PopOutEndpointViewModelFactory factory,
        IPingUpdateNotifier pingUpdateNotifier, IEndpointUpdateNotifier updateNotifier)
    {
        foreach (var model in popOutManager.GetEndpointModels()) PingViewModels.Add(factory.Create(model));

        _subscriptions =
        [
            updateNotifier.SubscribeToUpdate((model, _) =>
            {
                GetValueOrDefault(model.Id)?.UpdateFromModel(model);
                return ValueTask.CompletedTask;
            }),
            updateNotifier.SubscribeToDeletion((id, ct) =>
            {
                PingViewModels.Remove(PingViewModels.FirstOrDefault(p => p.Id == id)!);
                return ValueTask.CompletedTask;
            }),
            pingUpdateNotifier.SubscribeToPingSent((id, _) =>
            {
                GetValueOrDefault(id)?.OnPingSent();
                return ValueTask.CompletedTask;
            }),
            pingUpdateNotifier.SubscribeToPingUpdated((pingUpdated, _) =>
            {
                GetValueOrDefault(pingUpdated.Id)?.OnPingReceived(pingUpdated);
                return ValueTask.CompletedTask;
            })
        ];

        popOutManager.EndpointAdded += (_, e) => PingViewModels.Add(factory.Create(e));
        popOutManager.EndpointRemoved +=
            (_, id) => PingViewModels.Remove(PingViewModels.FirstOrDefault(p => p.Id == id)!);
    }

    public void Dispose()
    {
        foreach (var subscription in _subscriptions)
            subscription.Dispose();
    }

    public ObservableCollection<IPopOutEndpointViewModel> PingViewModels { get; } = [];

    private IPopOutEndpointViewModel? GetValueOrDefault(string id)
    {
        return PingViewModels.FirstOrDefault(vm => vm.Id == id);
    }

    partial void OnOpacityChanging(double value)
    {
        if (value is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(value), $"{value} is not in [0;1] range");
    }
}
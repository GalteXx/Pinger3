using System;
using System.Collections.Generic;
using Pinger3.ViewModels.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Pinger3.Services;
using Pinger3.Services.Pinging;
using Pinger3.Services.PopOut;
using Pinger3.Services.PopOut.Factories;

namespace Pinger3.ViewModels.PageViewModels;

public sealed partial class PopoutWindowViewModel : ObservableObject, IPopoutWindowViewModel, IDisposable
{
    private readonly List<IDisposable> _subscriptions;

    //TODO: BindStyles
    [ObservableProperty] private bool _clickThrough;
    [ObservableProperty] private string _selectedStyleClassName = "CompactResponseTime";
    [ObservableProperty] private double _opacity = 1;


    public PopoutWindowViewModel(IPopOutManager popOutManager, PopOutEndpointViewModelFactory factory,
        IPingUpdateNotifier pingUpdateNotifier, IEndpointUpdateNotifier updateNotifier)
    {
        foreach (var model in popOutManager.GetEndpointModels())
        {
            PingViewModels.Add(factory.Create(model));
        }

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

    private IPopOutEndpointViewModel? GetValueOrDefault(string id)
    {
        return PingViewModels.FirstOrDefault(vm => vm.Id == id);
    }

    public ObservableCollection<IPopOutEndpointViewModel> PingViewModels { get; } = [];

    partial void OnOpacityChanging(double value)
    {
        if (value is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(value), $"{value} is not in [0;1] range");
    }

    public void Dispose()
    {
        foreach (var subscription in _subscriptions)
            subscription.Dispose();
    }
}
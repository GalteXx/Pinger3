using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Pinger3.Models;

namespace Pinger3.ViewModels.Controls;

public partial class PopOutEndpointViewModel(string id, string address, string name)
    : ObservableObject, IPopOutEndpointViewModel
{
    [ObservableProperty] private string _address = address;
    private DateTime _lastPingTime;

    [ObservableProperty] private string _name = name;
    [ObservableProperty] private TimeSpan _ping = TimeSpan.FromMilliseconds(-1);

    private bool _pingInProgress;
    [ObservableProperty] private TimeSpan _timeSinceLastRequest = TimeSpan.FromMilliseconds(-1);
    public string Id { get; } = id;


    public void UpdateFromModel(EndpointModel model)
    {
        if (model.Id != Id)
            return;
        Name = model.Name;
    }

    public void UpdateTimeSinceLastRequest()
    {
        if (_pingInProgress)
            TimeSinceLastRequest = DateTime.Now - _lastPingTime;
    }

    public void OnPingReceived(PingUpdated update)
    {
        Ping = update.Ping;
        _pingInProgress = false;
        TimeSinceLastRequest = TimeSpan.Zero;
    }

    public void OnPingSent()
    {
        TimeSinceLastRequest = TimeSpan.Zero;
        _pingInProgress = true;
        _lastPingTime = DateTime.Now;
    }
}
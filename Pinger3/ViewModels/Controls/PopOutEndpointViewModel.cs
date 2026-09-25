using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Pinger3.Models;

namespace Pinger3.ViewModels.Controls;

public partial class PopOutEndpointViewModel(string id, string name)
    : ObservableObject
{
    public string Id { get; set; } = id;
    [ObservableProperty] private string _name = name;
    [ObservableProperty] private TimeSpan _ping = TimeSpan.FromMilliseconds(-1);
    [ObservableProperty] private TimeSpan _timeSinceLastRequest = TimeSpan.FromMilliseconds(-1);

    //the step shenanigans deserve their own service atp 
    private int _step = 0;

    public void UpdateFromModel(EndpointModel model)
    {
        if (model.Id != Id)
            return;
        Name = model.Name;
    }

    public void UpdateTimeSinceLastRequest(TimeSpan tick)
    {
        TimeSinceLastRequest += tick * _step;
    }

    public void OnPingReceived(PingUpdated update)
    {
        Ping = update.Ping;
        _step = 0;
    }

    public void OnPingSent()
    {
        TimeSinceLastRequest = TimeSpan.Zero;
        _step = 1;
    }
}
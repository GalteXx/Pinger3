using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Pinger3.Models;

namespace Pinger3.ViewModels.Controls;

public partial class PingingTargetViewModel : ObservableObject, IPingingTargetViewModel
{
    [ObservableProperty] private string _address = string.Empty;
    [ObservableProperty] private TimeSpan _delayBetweenRequests;
    [ObservableProperty] private string _id;
    [ObservableProperty] private bool _isActive;
    [ObservableProperty] private bool _isPopOut;

    private DateTime _lastPingTime = DateTime.Now;

    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private TimeSpan _ping;

    private bool _pingInProgress;
    [ObservableProperty] private TimeSpan _timeSinceLastRequest = TimeSpan.FromSeconds(-1);

    public PingingTargetViewModel(EndpointModel model, bool isActive, bool isPopOut)
    {
        _id = model.Id;
        TimeSinceLastRequest = TimeSpan.Zero;
        Name = model.Name;
        Address = model.Address;
        DelayBetweenRequests = model.DelayBetweenRequests;
        IsActive = isActive;
        IsPopOut = isPopOut;
    }

    public void UpdateFromModel(EndpointModel endpointModel)
    {
        if (Id != endpointModel.Id)
            return;
        Name = endpointModel.Name;
        Address = endpointModel.Address;
        DelayBetweenRequests = endpointModel.DelayBetweenRequests;
    }

    public void UpdateTimeSinceLastRequest()
    {
        if (_pingInProgress)
            TimeSinceLastRequest += DateTime.Now - _lastPingTime;
    }

    public void OnPingReceived(PingUpdated update)
    {
        Ping = update.Ping;
        _pingInProgress = false;
    }

    public void OnPingSent()
    {
        TimeSinceLastRequest = TimeSpan.Zero;
        _lastPingTime = DateTime.Now;
        _pingInProgress = true;
    }
}
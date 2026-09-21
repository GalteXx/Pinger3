using CommunityToolkit.Mvvm.ComponentModel;
using Pinger3.Models;
using System;

namespace Pinger3.ViewModels.Controls;

public partial class PingingTargetViewModel : ObservableObject, IPingingTargetViewModel
{

    public PingingTargetViewModel(EndpointModel model)
    {
        _id = model.Id;
        TimeSinceLastRequest = TimeSpan.Zero;
        Name = model.Name;
        DomainOrAddress = model.Address;
        DelayBetweenRequests = model.DelayBetweenRequests;
    }

    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _domainOrAddress = string.Empty;
    [ObservableProperty] private TimeSpan _ping;
    [ObservableProperty] private bool _isActive;
    [ObservableProperty] private TimeSpan _timeSinceLastRequest;
    [ObservableProperty] private string _id;
    [ObservableProperty] private TimeSpan _delayBetweenRequests;
    
    public void UpdateModel(EndpointModel endpointModel)
    {
        if (Id != endpointModel.Id)
            return;
        Name =  endpointModel.Name;
        DomainOrAddress = endpointModel.Address;
        DelayBetweenRequests = endpointModel.DelayBetweenRequests;
    }
}
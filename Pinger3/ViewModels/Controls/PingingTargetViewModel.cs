using CommunityToolkit.Mvvm.ComponentModel;
using Pinger3.Models;
using System;

namespace Pinger3.ViewModels.Controls
{
    public partial class PingingTargetViewModel : ObservableObject, IPingingTargetViewModel
    {
        private readonly EndpointModel _model;

        public PingingTargetViewModel(EndpointModel model)
        {
            TimeSinceLastRequest = TimeSpan.Zero;
            _model = model;
            Name = _model.Name;
            DomainOrAddress = _model.Address;
        }

        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private string _domainOrAddress = string.Empty;
        [ObservableProperty] private TimeSpan _ping;
        [ObservableProperty] private bool _isActive;
        [ObservableProperty] private TimeSpan _timeSinceLastRequest;

        public TimeSpan DelayBetweenRequests => _model.DelayBetweenRequests;
    }
}
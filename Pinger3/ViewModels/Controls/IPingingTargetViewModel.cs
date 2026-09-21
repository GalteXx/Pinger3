using CommunityToolkit.Mvvm.Input;
using Pinger3.Models;
using System;
using System.ComponentModel;

namespace Pinger3.ViewModels.Controls
{
    public interface IPingingTargetViewModel : INotifyPropertyChanged
    {
        string Id { get; }
        string DomainOrAddress { get; }
        string Name { get; }
        TimeSpan Ping { get; }
        TimeSpan TimeSinceLastRequest { get; }
        TimeSpan DelayBetweenRequests { get; }
        bool IsActive { get; set; }

        public void UpdateModel(EndpointModel endpointModel);
        void UpdateTimeSinceLastRequest(TimeSpan updateTime);
        void OnPingReceived(PingUpdated update);
        void OnPingSent();
    }
}
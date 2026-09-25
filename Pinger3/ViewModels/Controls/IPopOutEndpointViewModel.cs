using System;
using System.ComponentModel;
using Pinger3.Models;

namespace Pinger3.ViewModels.Controls;

public interface IPopOutEndpointViewModel : INotifyPropertyChanged
{
    string Id { get; }

    string Name { get; }

    TimeSpan Ping { get; }

    TimeSpan TimeSinceLastRequest { get; }

    void UpdateFromModel(EndpointModel model);
    void UpdateTimeSinceLastRequest(TimeSpan tick);
    void OnPingReceived(PingUpdated update);
    void OnPingSent();
}
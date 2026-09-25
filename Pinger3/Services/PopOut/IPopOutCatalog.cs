using System;
using Pinger3.Models;

namespace Pinger3.Services.PopOut;

public interface IPopOutCatalog
{
    event EventHandler<EndpointModel>? EndpointAdded;
    event EventHandler<string>? EndpointRemoved;
    event EventHandler<EndpointModel>? EndpointUpdated;
    event EventHandler<PingUpdated>? PingReceived;
    event EventHandler<string>? PingSent;
    void Add(string id);
    void Remove(string id);
}
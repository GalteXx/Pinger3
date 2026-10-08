using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.PopOut;

public interface IPopOutCatalog
{
    event EventHandler<EndpointModel>? EndpointAdded;
    event EventHandler<string>? EndpointRemoved;
    event EventHandler<EndpointModel>? EndpointUpdated;
    event EventHandler<PingUpdated>? PingReceived;
    event EventHandler<string>? PingSent;
    Task Add(string id);
    Task Remove(string id);
    bool IsPopOut(string id);
    Task Toggle(string id);
    IEnumerable<EndpointModel> GetEndpoints();
}
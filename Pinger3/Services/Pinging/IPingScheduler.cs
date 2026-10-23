using System;
using System.Collections.Generic;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public interface IPingScheduler
{
    void AddToSchedule(EndpointRuntime endpoint);
    void RemoveFromSchedule(string endpointId);
    event EventHandler<string>? PingSent;
    event EventHandler<PingUpdated>? PingReceived;
    void UpdateInSchedule(string id, ResolvedEndpointUpdate update);
}
using System;
using System.Collections.Generic;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public interface IPingScheduler
{
    Dictionary<string, EndpointRuntime> Endpoints { get; }
    event EventHandler<string>? PingSent;
    event EventHandler<PingUpdated>? PingReceived;
}
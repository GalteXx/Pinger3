using System;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public interface IPingScheduler
{
    event EventHandler<EndpointUpdated>? PingCompleted;
    void AddModel(EndpointModel model);
    void RemoveModel(EndpointModel model);
}
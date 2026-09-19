using System;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

internal interface IPingTransport
{
    Task<TimeSpan> PingAsync(EndpointModel endpoint, CancellationToken ct);
}
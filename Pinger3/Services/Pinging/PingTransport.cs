using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

internal class PingTransport : IPingTransport
{
    public async Task<TimeSpan> PingAsync(EndpointRuntime endpoint, CancellationToken ct)
    {
        Ping ping = new();
        var reply = await ping.SendPingAsync(endpoint.Address);

        return reply.Status != IPStatus.Success
            ? TimeSpan.FromSeconds(-1)
            : TimeSpan.FromMilliseconds(reply.RoundtripTime);
    }
}
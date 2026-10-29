using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

internal class PingTransport : IPingTransport
{
    public async Task<TimeSpan> PingAsync(EndpointRuntime endpoint, CancellationToken ct)
    {
        using Ping ping = new();

        PingReply? reply = null;
        try
        {
            reply = await ping.SendPingAsync(endpoint.Address);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception)
        {
            // Oh wow, I don't have a logger.
            // TODO: report exceptions back to UI
        }

        return reply?.Status != IPStatus.Success
            ? TimeSpan.FromSeconds(-1)
            : TimeSpan.FromMilliseconds(reply.RoundtripTime);
    }
}
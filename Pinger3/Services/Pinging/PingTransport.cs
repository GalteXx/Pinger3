using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

internal class PingTransport(AddressResolver resolver) : IPingTransport
{
    public async Task<TimeSpan> PingAsync(EndpointModel endpoint, CancellationToken ct)
    {
        Ping ping = new();
        
        var address = (await resolver.ResolveAddresses(endpoint, ct)).FirstOrDefault();
        if (address == null)
            return TimeSpan.FromSeconds(-1);

        var reply = await ping.SendPingAsync(address);

        return reply.Status != IPStatus.Success
            ? TimeSpan.FromSeconds(-1)
            : TimeSpan.FromMilliseconds(reply.RoundtripTime);
    }
}
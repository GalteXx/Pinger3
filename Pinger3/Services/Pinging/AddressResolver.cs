using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public class AddressResolver
{
    public async Task<IPAddress[]> ResolveAddressesAsync(EndpointModel model, CancellationToken ct)
    {
        try
        {
            return await Dns.GetHostAddressesAsync(model.Address, ct);
        }
        catch
        {
            // TODO: propagate dns resolution failure to UI
            return [IPAddress.None];
        }
    }
}
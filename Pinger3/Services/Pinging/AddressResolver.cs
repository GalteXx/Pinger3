using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public class AddressResolver
{
    public Task<IPAddress[]> ResolveAddresses(EndpointModel model, CancellationToken ct)
    {
        return Dns.GetHostAddressesAsync(model.Address, ct);
    }
}
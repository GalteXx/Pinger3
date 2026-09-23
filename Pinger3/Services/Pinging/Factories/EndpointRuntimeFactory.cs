using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public class EndpointRuntimeFactory(AddressResolver resolver)
{
    public async Task<EndpointRuntime> CreateRuntimeAsync(EndpointModel model, CancellationToken ct)
    {
        var address = (await resolver.ResolveAddressesAsync(model, ct)).FirstOrDefault() ?? IPAddress.None;
        return new EndpointRuntime(model.Id, address, model.DelayBetweenRequests);
    }
}
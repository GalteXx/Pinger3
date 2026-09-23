using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services.Pinging;

public class EndpointUpdateResolver(AddressResolver resolver)
{
    public async Task<ResolvedEndpointUpdate?> ResolveAsync(EndpointModel model, CancellationToken ct)
    {
        var address = (await resolver.ResolveAddressesAsync(model, ct)).FirstOrDefault();
        return address is null ? null : new ResolvedEndpointUpdate(address, model.DelayBetweenRequests);
    }
}
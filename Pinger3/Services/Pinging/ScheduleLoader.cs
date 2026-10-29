using System.Threading;
using System.Threading.Tasks;

namespace Pinger3.Services.Pinging;

public class ScheduleLoader(
    IActiveEndpointsRegistry registry,
    IEndpointRepository endpointRepository,
    IPingScheduler scheduler,
    IEndpointRuntimeFactory factory)
{
    public async Task LoadScheduleAsync(CancellationToken ct)
    {
        foreach (var id in registry.GetActiveIds())
        {
            ct.ThrowIfCancellationRequested();
            var endpointModel = endpointRepository.GetEndpoint(id);
            if (endpointModel == null) continue;
            var runtime = await factory.CreateRuntimeAsync(endpointModel, ct);
            scheduler.AddToSchedule(runtime);
        }
    }
}
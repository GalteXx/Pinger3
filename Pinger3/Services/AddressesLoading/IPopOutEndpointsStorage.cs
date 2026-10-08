using System.Threading.Tasks;

namespace Pinger3.Services;

public interface IPopOutEndpointsStorage
{
    Task AddPopOutEndpointAsync(string id);
    Task RemovePopOutEndpointAsync(string id);
}
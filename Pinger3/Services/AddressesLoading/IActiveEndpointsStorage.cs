using System.Threading.Tasks;

namespace Pinger3.Services;

public interface IActiveEndpointsStorage
{
    Task AddActiveEndpointAsync(string id);
    Task RemoveActiveEndpointAsync(string id);
}
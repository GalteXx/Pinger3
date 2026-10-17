using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pinger3.Services;

public interface IPopOutEndpointsStorage
{
    Task SavePopOutEndpointAsync(string id);
    Task RemovePopOutEndpointAsync(string id);
    IAsyncEnumerable<string> GetAllPopOutEndpointsAsync();
}
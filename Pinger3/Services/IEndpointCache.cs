using Pinger3.Models;

namespace Pinger3.Services;

public interface IEndpointCache
{
    bool TryGetValue(string key, out EndpointModel? value);
    void Remove(string key);
    void AddOrUpdateValue(EndpointModel value);
}
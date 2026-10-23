using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Pinger3.Services;

public class EndpointsMembershipCache : IActiveEndpointsCache, IPopOutEndpointsCache
{
    private readonly ConcurrentDictionary<string, string> _activeEndpoints = new();
    private readonly ConcurrentDictionary<string, string> _popOutEndpoints = new();

    IEnumerable<string> IActiveEndpointsCache.GetAllIds() => _activeEndpoints.Keys;

    void IActiveEndpointsCache.AddId(string id) => _activeEndpoints.TryAdd(id, id);

    void IActiveEndpointsCache.RemoveId(string id) => _activeEndpoints.TryRemove(id, out _);

    bool IActiveEndpointsCache.IsStored(string id) => _activeEndpoints.ContainsKey(id);
    IEnumerable<string> IPopOutEndpointsCache.GetAllIds() => _popOutEndpoints.Keys;

    void IPopOutEndpointsCache.AddId(string id) => _popOutEndpoints.TryAdd(id, id);

    void IPopOutEndpointsCache.RemoveId(string id) => _popOutEndpoints.TryRemove(id, out _);

    bool IPopOutEndpointsCache.IsStored(string id) => _popOutEndpoints.ContainsKey(id);
}
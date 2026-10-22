using System.Collections.Generic;

namespace Pinger3.Services;

public class EndpointsMembershipCache : IActiveEndpointsCache, IPopOutEndpointsCache
{
    private readonly HashSet<string> _activeEndpoints = [];
    private readonly HashSet<string> _popOutEndpoints = [];

    void IActiveEndpointsCache.AddId(string id)
    {
        _activeEndpoints.Add(id);
    }

    void IPopOutEndpointsCache.RemoveId(string id)
    {
        _activeEndpoints.Remove(id);
    }

    bool IPopOutEndpointsCache.IsStored(string id)
    {
        return _activeEndpoints.Contains(id);
    }

    void IPopOutEndpointsCache.AddId(string id)
    {
        _popOutEndpoints.Add(id);
    }

    void IActiveEndpointsCache.RemoveId(string id)
    {
        _popOutEndpoints.Remove(id);
    }

    bool IActiveEndpointsCache.IsStored(string id)
    {
        return _popOutEndpoints.Contains(id);
    }
}
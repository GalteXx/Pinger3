using System.Collections.Concurrent;
using System.Collections.Generic;
using Pinger3.Models;

namespace Pinger3.Services;

public class EndpointCache : IEndpointCache
{
    private readonly ConcurrentDictionary<string, EndpointModel> _endpoints = new();

    public bool TryGetValue(string key, out EndpointModel? value) => _endpoints.TryGetValue(key, out value);

    public EndpointModel? GetValueOrDefault(string id) => _endpoints.GetValueOrDefault(id);

    public void Remove(string key)
    {
        _endpoints.Remove(key, out _);
    }

    public void AddOrUpdateValue(EndpointModel value)
    {
        _endpoints.AddOrUpdate(value.Id, value, (_, _) => value);
    }
}
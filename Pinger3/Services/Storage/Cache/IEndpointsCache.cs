using System.Collections.Generic;
using Pinger3.Models;

namespace Pinger3.Services;

public interface IEndpointsCache
{
    bool TryGetValue(string key, out EndpointModel? value);
    void Remove(string key);
    void AddOrUpdateValue(EndpointModel value);
    IEnumerable<EndpointModel> GetAllEndpoints();
    EndpointModel? GetValueOrDefault(string id);
}
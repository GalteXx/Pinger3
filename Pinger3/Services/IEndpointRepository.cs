using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public interface IEndpointRepository
{
    ReadOnlyDictionary<string, EndpointModel> CachedEndpoints { get; }
    event EventHandler<string>? EndpointUpdated;
    Task LoadFromSource();
    Task Add(EndpointModel endpoint);
    EndpointModel? GetEndpoint(string id);
}
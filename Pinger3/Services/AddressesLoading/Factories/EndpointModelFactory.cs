using System;
using Pinger3.DataTypes;
using Pinger3.Models;

namespace Pinger3.Services;

public class EndpointModelFactory
{
    public EndpointModel CreateValidEndpointModel(EndpointDto endpoint)
    {
        var delay = endpoint.RequestDelay == null
            ? TimeSpan.FromSeconds(1)
            : TimeSpan.FromMilliseconds(Convert.ToInt32(endpoint.RequestDelay));
        return new EndpointModel(endpoint.Id ?? ShortGuid.NewShortGuid(), endpoint.Name ?? "", endpoint.Address ?? "",
            delay);
    }
}
using System;
using Pinger3.Models;

namespace Pinger3.Services;

// At that point, I believe there is no need for "validators" as a checker for some private data
// I will be using backup data for unparsable entries, but it's not like there are many.
public class EndpointModelFactory
{
    public EndpointModel CreateValidEndpointConfig(EndpointDto endpoint)
    {
        // ConfigUpdater (To be implemented) nullifies the need for ID check,
        // unless User manually enters 2 identical ids. But at that point, the named user deserves broken app and data loss
        if (!TimeSpan.TryParse(endpoint.RequestDelay, out var delay))
            delay = TimeSpan.FromSeconds(1);

        return new EndpointModel(endpoint.Name ?? "", endpoint.Address ?? "", delay);
    }
}
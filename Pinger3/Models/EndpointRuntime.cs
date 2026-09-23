using System;
using System.Net;

namespace Pinger3.Models;

public class EndpointRuntime(string id, IPAddress address, TimeSpan delay)
{
    public string Id { get; private set; } = id;
    public IPAddress Address { get; private set; } = address;
    public TimeSpan DelayBetweenRequests { get; private set; } = delay;
    public DateTime LastPinged { get; set; }
    public TimeSpan Ping { get; set; }

    public void UpdateAddress(ResolvedEndpointUpdate endpoint)
    {
        Address = endpoint.Address;
        DelayBetweenRequests = endpoint.DelayBetweenRequests;
    }
}
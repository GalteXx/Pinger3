using System;
using System.Net;

namespace Pinger3.Models;

public class EndpointRuntime
{
    public required string Id { get; init; }
    public required IPAddress Address { get; init; }
    public required TimeSpan DelayBetweenRequests { get;  init; }
    public DateTime LastPinged { get; set; }
    public TimeSpan Ping { get; set; }
}
using System;

namespace Pinger3.Models;

public class EndpointRuntime
{
    public required EndpointModel Model { get; init; }
    public DateTime LastPinged { get; set; }
    public TimeSpan Ping { get; set; }
    public bool IsActive { get; set; }
}
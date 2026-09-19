using System;

namespace Pinger3.Models;

public record EndpointUpdated(string Id, TimeSpan Ping, DateTime LastPinged)
{ }
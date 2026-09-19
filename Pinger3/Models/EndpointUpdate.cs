using System;

namespace Pinger3.Models;

public record EndpointUpdated(TimeSpan Ping, DateTime LastPinged)
{ }
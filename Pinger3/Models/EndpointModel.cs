using System;

namespace Pinger3.Models;

public sealed record EndpointModel(string Id, string Name, string Address, TimeSpan DelayBetweenRequests);
using System;
using System.Net;

namespace Pinger3.Models;

public record ResolvedEndpointUpdate(IPAddress Address, TimeSpan DelayBetweenRequests)
{ }
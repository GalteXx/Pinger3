using System;

namespace Pinger3.Models;

public record PingUpdated(string Id, TimeSpan Ping)
{ }
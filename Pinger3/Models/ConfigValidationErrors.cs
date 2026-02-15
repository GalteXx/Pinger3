using System;

namespace Pinger3.Models
{
    [Flags]
    public enum ConfigValidationErrors
    {
        None = 0,
        MissingName = 1 << 0,
        MissingAddress = 1 << 1,
        InvalidIpFormat = 1 << 2,
        DomainUnresolvable = 1 << 3,
        BothIpAndDomainProvided = 1 << 4
    }
}

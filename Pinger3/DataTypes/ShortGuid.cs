using System;

namespace Pinger3.DataTypes;

/// <summary>
/// Represents considerably shorter version of GUID, with a collision chance 1:2100
/// Further reinforces my suspicion I should've used SQLite
/// </summary>
public static class ShortGuid
{
    public static string NewShortGuid()
    {
        Span<byte> bytes = stackalloc byte[16];
        Guid.NewGuid().TryWriteBytes(bytes);
        
        return Convert.ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }
}
using System;

namespace Pinger3.Models
{
    public struct AddressDTO(string? id, string? name, string? address, string? requestDelay)
    {
        public string? Id { get; } = id;
        public string? Name { get; } = name;
        public string? Address { get; } = address;
        public string? RequestDelay { get; } = requestDelay;
    }
}
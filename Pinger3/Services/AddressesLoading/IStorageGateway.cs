using System.Collections.Generic;
using System.Threading.Tasks;
using Pinger3.Models;

namespace Pinger3.Services;

public interface IStorageGateway
{
    IAsyncEnumerable<EndpointDto> ReadAddressAsync();
    Task WriteAddressAsync(EndpointDto dto);
    Task WriteActiveEndpointAsync(string id);
    Task EraseActiveEndpointAsync(string id);
    Task WritePopOutEndpointAsync(string id);
    Task ErasePopOutEndpointAsync(string id);
}
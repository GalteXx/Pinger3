using System.Collections.Generic;
using Pinger3.Models;

namespace Pinger3.Services;

public interface IStorageGateway
{
    IAsyncEnumerable<EndpointDto> ReadAddressAsync();
}
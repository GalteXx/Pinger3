using System.Collections.Generic;
using System.Xml.Linq;
using Pinger3.Models;

namespace Pinger3.Services;

public class XmlStorageReader : IStorageReader
{
    private readonly XmlAddressesStorageLoader _loader = new();

    public async IAsyncEnumerable<EndpointDto> ReadAddressAsync()
    {
        var storage = await _loader.LoadStorageAsync();
        foreach (var el in storage.Elements())
        {
            var dto = CreateAddressDto(el);
            if (dto == null) // there is literally not a single case when this could fire 
                continue;
            yield return dto;
        }
    }

    private static EndpointDto? CreateAddressDto(XElement el)
    {
        var id = el.Attribute("id")?.Value;
        var name = el.Attribute("Name")?.Value;
        var address = el.Attribute("Address")?.Value;
        var delay = el.Attribute("")?.Value;
        
        return new EndpointDto(id, name, address, delay);
    }
}
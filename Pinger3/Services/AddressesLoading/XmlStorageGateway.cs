using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Pinger3.Models;

namespace Pinger3.Services;

public class XmlStorageGateway : IStorageGateway
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
        var delay = el.Attribute("Delay")?.Value;

        return new EndpointDto(id, name, address, delay);
    }

    private static XElement CreateXElement(EndpointDto dto)
    {
        var el = new XElement("Endpoint");
        el.SetAttributeValue("Id", dto.Id);
        el.SetAttributeValue("Name", dto.Name);
        el.SetAttributeValue("Address", dto.Address);
        el.SetAttributeValue("Delay", dto.RequestDelay);
        return el;
    }

    public async Task WriteAddressAsync(EndpointDto dto)
    {
        var storage = await _loader.LoadStorageAsync();

        var endpointElement = CreateXElement(dto);
        var replacedElement = storage.Elements().FirstOrDefault(el => el.Attribute("id")?.Value == dto.Id);
        if (replacedElement != null)
            replacedElement.ReplaceWith(endpointElement);
        else
            storage.Add(endpointElement);
        
        await _loader.SaveAsync(storage);
    }
}
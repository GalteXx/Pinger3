using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Pinger3.Models;

namespace Pinger3.Services;

public class XmlStorageGateway : IStorageGateway
{
    private readonly XmlAddressesStorageLoader _loader = new();

    private static EndpointDto? CreateAddressDto(XElement el)
    {
        var id = el.Attribute("Id")?.Value;
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

    private static XElement? GetElementById(string id, XElement endpointsEntry)
    {
        return endpointsEntry
            .Elements("EndpointID")
            .FirstOrDefault(x => (string?)x.Attribute("Id") == id);
    }

    private static XElement? GetEndpointsSet(SetType type, XDocument storage)
    {
        var typeName = Enum.GetName(typeof(SetType), type); // TODO: log nulls
        return typeName is null ? null : storage.Root?.Element(typeName);
    }

    public async IAsyncEnumerable<EndpointDto> ReadAddressAsync()
    {
        var storage = await _loader.LoadStorageAsync();
        foreach (var el in storage.Root?.Element("Endpoints")?.Elements() ?? [])
        {
            var dto = CreateAddressDto(el);
            if (dto == null) // there is literally not a single case when this could fire 
                continue;
            yield return dto;
        }
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

    public Task WriteActiveEndpointAsync(string id) =>
        ModifyEndpointsSetAsync(SetType.Active, set => AddEndpointId(set, id));

    public Task EraseActiveEndpointAsync(string id) =>
        ModifyEndpointsSetAsync(SetType.Active, set => RemoveEndpointId(set, id));

    public Task WritePopOutEndpointAsync(string id) =>
        ModifyEndpointsSetAsync(SetType.PopOut, set => AddEndpointId(set, id));

    public Task ErasePopOutEndpointAsync(string id) =>
        ModifyEndpointsSetAsync(SetType.PopOut, set => RemoveEndpointId(set, id));

    private async Task ModifyEndpointsSetAsync(SetType type, Func<XElement, bool> mutate)
    {
        var storage = await _loader.LoadStorageAsync();
        var set = GetEndpointsSet(type, storage);
        if (set is null)
            return; // TODO: log

        if (mutate(set))
            await _loader.SaveAsync(storage);
        // TODO: else: log
    }

    private static bool AddEndpointId(XElement set, string id)
    {
        if (GetElementById(id, set) is not null)
            return false;

        set.Add(new XElement("EndpointID", new XAttribute("Id", id)));
        return true;
    }

    private static bool RemoveEndpointId(XElement set, string id)
    {
        var element = GetElementById(id, set);
        if (element is null)
            return false;

        element.Remove();
        return true;
    }

    private enum SetType
    {
        Active,
        PopOut
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Pinger3.DataTypes;
using Pinger3.Models;

namespace Pinger3.Services.Migration;

public class MigrationFromV0 : IConfigMigration
{
    public string? FromVersion => null;
    public string ToVersion => "1.0";


    private IEnumerable<EndpointDto> GetEndpoints(XDocument xDocument)
    {
        var element = xDocument.Root?.Element("TargetIPs");
        if (element == null)
            yield break;
        foreach (var endpointElement in element.Elements())
        {
            var name = endpointElement.Attribute("Name")?.Value;
            // I give priority to explicit Ip for no particular reason
            var address = endpointElement.Attribute("IP")?.Value ?? endpointElement.Attribute("Domain")?.Value;
            var delay = endpointElement.Attribute("Delay")?.Value;
            yield return new EndpointDto(null, name, address, delay);
        }
    }

    public XDocument Migrate(XDocument document)
    {
        return new XDocument(
            new XElement("Data",
                new XAttribute("Version", "1.0"),
                new XElement("Endpoints",
                    GetEndpoints(document).Select(e =>
                        new XElement("Endpoint",
                            new XAttribute("Id", ShortGuid.NewShortGuid()),
                            new XAttribute("Name", e.Name ?? string.Empty),
                            new XAttribute("Address", e.Address ?? string.Empty),
                            new XAttribute("Delay", e.RequestDelay ?? "5000")
                        )
                    )
                ),
                new XElement("Active"),
                new XElement("PopOut")
            )
        );
    }
}
using System.Xml.Linq;

namespace Pinger3.Services.Migration;

public interface IConfigMigration
{
    string? FromVersion { get; }
    string ToVersion { get; }

    XDocument Migrate(XDocument document);
}
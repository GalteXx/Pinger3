using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pinger3.Services.Migration;

public class PersistentDataMigrator
{
    private readonly XmlAddressesStorageLoader _loader;
    private readonly List<IConfigMigration> _configMigrations;

    public PersistentDataMigrator()
    {
        _loader = new XmlAddressesStorageLoader();
        _configMigrations = [new MigrationFromV0()]; // aye, I'll just have a list of all of them
    }

    public async Task MigrateAsync()
    {
        var doc = await _loader.LoadStorageAsync();
        var version = doc.Root?.Element("Data")?.Attribute("Version")?.Value;
        while (true)
        {
            var migration = _configMigrations.FirstOrDefault(m => m.FromVersion == version);
            if (migration == null)
                break;
            version = migration.ToVersion;
            doc = migration.Migrate(doc);
        }

        await _loader.SaveAsync(doc);
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pinger3.Services.Migration;

public class PersistentDataMigrator
{
    private readonly XmlAddressesStorageLoader _loader = new();
    // aye, I'll just have a list of all of them
    private readonly List<IConfigMigration> _configMigrations = [new MigrationFromV0()]; 

    public async Task MigrateAsync()
    {
        var doc = await _loader.LoadStorageAsync();
        var version = doc.Root?.Attribute("Version")?.Value;
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
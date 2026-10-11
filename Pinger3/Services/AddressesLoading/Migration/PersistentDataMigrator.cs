using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Pinger3.Services.Migration;

public class PersistentDataMigrator(SqliteConnectionFactory connectionFactory)
{
    private readonly IEnumerable<IStorageMigration> _configMigrations =
    [
        new InitialSqliteMigration()
    ];


    public async Task MigrateAsync()
    {
        while (true)
        {
            await using var connection = connectionFactory.Create();
            var version = await GetVersionAsync(connection);
            var migration = _configMigrations.FirstOrDefault(m => m.FromVersion == version);
            if (migration == null)
                break;

            await migration.Migrate(connection);
        }
    }

    private static async Task<int> GetVersionAsync(SqliteConnection connection)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA user_version;";

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }
}
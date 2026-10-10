using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Pinger3.Services;

public class SqliteDatabaseInitializer(SqliteConnectionFactory connectionFactory) : IStorageInitializer
{
    public async Task Initialize()
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await CreateEndpointsTable(connection);
        await CreateEndpointRolesTable(connection);
    }

    private static async Task CreateEndpointRolesTable(SqliteConnection connection)
    {
        var createRolesTable = connection.CreateCommand();
        createRolesTable.CommandText = """
                                       CREATE TABLE IF NOT EXISTS endpoint_role (
                                         role        TEXT NOT NULL CHECK (role IN ('active','popout')),
                                         endpoint_id TEXT NOT NULL REFERENCES endpoint(id),
                                         PRIMARY KEY (role, endpoint_id)
                                       );
                                       """;
        await createRolesTable.ExecuteNonQueryAsync();
    }

    private static async Task CreateEndpointsTable(SqliteConnection connection)
    {
        var createEndpointsTable = connection.CreateCommand();
        createEndpointsTable.CommandText = """
                                           CREATE TABLE IF NOT EXISTS endpoint (
                                             id        integer NOT NULL PRIMARY KEY,
                                             name TEXT NOT NULL,
                                             address TEXT NOT NULL,
                                             delay TEXT NOT NULL
                                           );
                                           """;
        await createEndpointsTable.ExecuteNonQueryAsync();
    }
}
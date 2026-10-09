using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Pinger3.Services;

public class SqliteDatabaseInitializer : IStorageInitializer
{
    private readonly string _directoryPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Geckosystem", "Pinger");

    public async Task Initialize()
    {
        Directory.CreateDirectory(_directoryPath);
        var connectionString = new SqliteConnectionStringBuilder()
        {
            DataSource = Path.Combine(_directoryPath, "Pinger3.sqlite"),
        }.ToString();
        await using var connection = new SqliteConnection(connectionString);
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
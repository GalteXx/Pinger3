using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Pinger3.Models;

namespace Pinger3.Services;

public class SqliteStorageGateway(SqliteConnectionFactory connectionFactory)
    : IStorageGateway
{
    public async IAsyncEnumerable<EndpointDto> GetAllEndpointsAsync()
    {
        var endpoints = QueryAsync("SELECT * FROM endpoint",
            reader => new EndpointDto
            (
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3)
            ));
        await foreach (var dto in endpoints)
            yield return dto;
    }

    public async Task<EndpointDto?> GetEndpoint(string id)
    {
        var endpoints = QueryAsync(
            "SELECT id, name, address, delay FROM endpoint WHERE id = @id",
            reader => new EndpointDto
            (
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3)
            ),
            new SqliteParameter("@id", id));

        await foreach (var dto in endpoints)
        {
            return dto;
        }

        return null;
    }

    public async Task EditEndpointAsync(EndpointDto dto) =>
        await ExecuteNonQueryCommandAsync(
            "UPDATE endpoint SET name = @name, address = @address, delay = @delay WHERE id = @id",
            new SqliteParameter("@id", dto.Id),
            new SqliteParameter("@name", dto.Name),
            new SqliteParameter("@address", dto.Name),
            new SqliteParameter("@delay", dto.RequestDelay));

    public async Task AddEndpointAsync(EndpointDto dto) =>
        await ExecuteNonQueryCommandAsync(
            "INSERT INTO endpoint (name, address, delay) VALUES (@name, @address, @delay)",
            new SqliteParameter("@id", dto.Id),
            new SqliteParameter("@name", dto.Name),
            new SqliteParameter("@address", dto.Name),
            new SqliteParameter("@delay", dto.RequestDelay));

    public async Task SaveActiveEndpointAsync(string id) =>
        await ExecuteNonQueryCommandAsync(
            "INSERT INTO active_endpoint (endpoint_id) VALUES (@endpoint_id)",
            new SqliteParameter("@endpoint_id", id));

    public async Task RemoveActiveEndpointAsync(string id) =>
        await ExecuteNonQueryCommandAsync(
            "DELETE FROM active_endpoint WHERE endpoint_id = @endpoint_id",
            new SqliteParameter("@endpoint_id", id));

    public async Task SavePopOutEndpointAsync(string id) =>
        await ExecuteNonQueryCommandAsync(
            "INSERT INTO popout_endpoint (endpoint_id) VALUES (@endpoint_id)",
            new SqliteParameter("@endpoint_id", id));

    public async Task RemovePopOutEndpointAsync(string id) =>
        await ExecuteNonQueryCommandAsync(
            "DELETE FROM popout_endpoint WHERE endpoint_id = @endpoint_id",
            new SqliteParameter("@endpoint_id", id));

    public IAsyncEnumerable<string> GetAllPopOutEndpointsAsync()
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<string> GetActiveEndpointsAsync()
    {
        throw new NotImplementedException();
    }

    private async Task<T> ExecuteAsync<T>(string commandText, Func<SqliteCommand, Task<T>> execute,
        params SqliteParameter[] parameters)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = commandText;

        if (parameters.Length > 0)
            command.Parameters.AddRange(parameters);

        return await execute(command);
    }

    private async IAsyncEnumerable<T> QueryAsync<T>(string commandText, Func<SqliteDataReader, T> map,
        params SqliteParameter[] parameters)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = commandText;
        if (parameters.Length > 0)
            command.Parameters.AddRange(parameters);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            yield return map(reader);
    }

    private Task<int> ExecuteNonQueryCommandAsync(string commandText, params SqliteParameter[] parameters) =>
        ExecuteAsync(commandText, cmd => cmd.ExecuteNonQueryAsync(), parameters);
}
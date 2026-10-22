using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Pinger3.Services;

public class SqliteConnectionFactory
{
    private readonly string _directoryPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Geckosystem", "Pinger");

    public SqliteConnection Create()
    {
        Directory.CreateDirectory(_directoryPath);
        var connectionString = new SqliteConnectionStringBuilder()
        {
            DataSource = Path.Combine(_directoryPath, "Pinger3.sqlite"),
        }.ToString();
        return new SqliteConnection(connectionString);
    }
}
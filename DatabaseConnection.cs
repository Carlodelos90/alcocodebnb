using System;
using Npgsql;

namespace alcocodebnb;
public class DatabaseConnection
{
    private readonly string _host = "localhost";
    private readonly string _port = "5432";
    private readonly string _username = "postgres";
    private readonly string _password = "abc123";
    private readonly string _database = "postgres";
    
    private NpgsqlDataSource _connection;

    public NpgsqlDataSource Connection()
    {
        return _connection;
    }

    public DatabaseConnection()
    {
        _connection = NpgsqlDataSource.Create($"Host={_host};Port={_port};Username={_username};Password={_password};Database={_database}");
        
        using var conn = _connection.OpenConnection(); // Kontrollerar att vi har lyckats kopplat upp oss till databasen.
    }
}
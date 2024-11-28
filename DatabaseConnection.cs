using System;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace alcocodebnb;
public class DatabaseConnection
{
    private readonly string _host = "localhost";
    private readonly string _port = "5432"; // Default port brukar vara 5432
    private readonly string _username = "postgres";
    private readonly string _password = "postgres";
    private readonly string _database = "postgres";
    /*
        Per default så använder man public-schemat, vill man ändra till ett annat schema
        lägger man till "SearchPath={schema_namn}" i slutet av Create-metodens Sträng.

        private readonly string _schema = "";
        _connection = NpgsqlDataSource.Create($"Host={_host};Port={_port};Username={_username};Password={_password};Database={_database};SearchPath={_schema}");
     */
    
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
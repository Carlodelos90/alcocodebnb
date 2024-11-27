using System;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace alcocodebnb
{
    public class DatabaseConnection
    {
        private readonly string? _connectionString;

        public DatabaseConnection()
        {
            // Load connection string securely from configuration
            var builder = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", optional: true)  // For app settings
                 .AddEnvironmentVariables(); // For environment variables
            


            IConfiguration configuration = builder.Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Database connection string is not configured.");
            }
        }

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public void TestConnection()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open(); // Attempt to open the connection
                Console.WriteLine("Connection successful!"); // Success message
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to the database: {ex.Message}");
                throw; // Re-throw the exception if necessary
            }
        }
    }
}
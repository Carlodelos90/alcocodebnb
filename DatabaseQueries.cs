using System; // Provides basic system functionalities like Console output
using Npgsql; // Allows us to connect to and interact with a PostgreSQL database

namespace alcocodebnb // The namespace groups related classes and functionality
{
    public class DatabaseQueries // A class to handle database queries
    {
        private readonly DatabaseConnection _dbConnection; // Stores the database connection object

        // Constructor: Accepts a DatabaseConnection instance and initializes the _dbConnection field
        public DatabaseQueries(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection; // Saves the provided connection object for later use
        }

        // Method: Fetches and displays all customer names from the "customer" table
        public void GetAllCustomers()
        {
            // SQL query to select only the "firstname" and "lastname" columns from the "customer" table
            string query = "SELECT firstname, lastname FROM customer;";

            try
            {
                // Step 1: Get a database connection from the DatabaseConnection class
                using var connection = _dbConnection.GetConnection();

                // Step 2: Open the database connection to prepare for the query
                connection.Open();

                // Step 3: Create a command object to execute the SQL query
                using var command = new NpgsqlCommand(query, connection);

                // Step 4: Execute the query and get the results in a data reader
                using var reader = command.ExecuteReader();

                // Step 5: Print the header for the list of customers
                Console.WriteLine("Customers List:");

                // Step 6: Loop through all rows in the result set
                while (reader.Read()) // Read() moves to the next row in the result set
                {
                    // Retrieve the "firstname" and "lastname" values from the current row
                    string firstName = reader.GetString(0); // GetString(0) fetches the value in the first column
                    string lastName = reader.GetString(1);  // GetString(1) fetches the value in the second column

                    // Print the full name to the console
                    Console.WriteLine($"- {firstName} {lastName}");
                }
            }
            catch (Exception ex) // Catch any errors that occur while working with the database
            {
                // Print an error message if something goes wrong
                Console.WriteLine($"Error fetching customers: {ex.Message}");
            }
        }
    }
}
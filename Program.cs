using System;

namespace alcocodebnb
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Menu menu = new Menu();
                //menu.Start();
                
                var dbConnection = new DatabaseConnection();
                var dbQueries = new DatabaseQueries(dbConnection);
                dbConnection.TestConnection(); // Test the connection and print the message
                dbQueries.GetAllCustomers();
                

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}




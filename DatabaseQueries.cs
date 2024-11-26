using System;
using Npgsql;

namespace alcocodebnb
{
    public class DatabaseQueries
    {
        private readonly DatabaseConnection _dbConnection;

        public DatabaseQueries(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void GetAllCustomers()
        {
            string query = "SELECT firstname, lastname FROM customer;";

            try
            {
                using var connection = _dbConnection.GetConnection();
                connection.Open();

                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                Console.WriteLine("Customers List:");
                while (reader.Read())
                {
                    string firstName = reader.GetString(0);
                    string lastName = reader.GetString(1);

                    Console.WriteLine($"- {firstName} {lastName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customers: {ex.Message}");
            }
        }

        public void GetAllBookings()
        {
            string query = "SELECT id, customerid, accommodationid, startdate, enddate, totalprice, numberofguests, status FROM booking;";

            try
            {
                using var connection = _dbConnection.GetConnection();
                connection.Open();

                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                Console.WriteLine("Bookings List:");
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int customerId = reader.GetInt32(1);
                    int accommodationId = reader.GetInt32(2);
                    DateTime startDate = reader.GetDateTime(3);
                    DateTime endDate = reader.GetDateTime(4);
                    decimal totalPrice = reader.GetDecimal(5);
                    int numberOfGuests = reader.GetInt32(6);
                    string status = reader.GetString(7);

                    Console.WriteLine($"Booking ID: {id}, Customer ID: {customerId}, Accommodation ID: {accommodationId}, Start Date: {startDate:yyyy-MM-dd}, End Date: {endDate:yyyy-MM-dd}, Total Price: {totalPrice:C}, Guests: {numberOfGuests}, Status: {status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching bookings: {ex.Message}");
            }
        }
    }
}
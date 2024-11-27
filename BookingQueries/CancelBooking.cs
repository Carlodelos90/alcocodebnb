namespace alcocodebnb.BookingQueries;
using Npgsql;

public class CancelBooking
{
    private DatabaseConnection _dbConnection;

    public CancelBooking(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public void ShowBookings()
    {
        string query = "SELECT * from booking;";

        try
        {
            using var connection = _dbConnection.GetConnection();
            connection.Open();

            using var command = new NpgsqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            Console.WriteLine("Booking List:");
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int customerid = reader.GetInt32(1);
                Console.WriteLine($"- {id} {customerid}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching bookings: {ex.Message}");
        }
    }
}
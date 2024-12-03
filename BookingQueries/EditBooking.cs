namespace alcocodebnb.BookingQueries;
using Npgsql;

public class EditBooking
{
    private static NpgsqlDataSource? _database;

    public EditBooking(NpgsqlDataSource database)
    {
        _database = database ?? throw new ArgumentNullException(nameof(database));
    }


    public static async Task<int> ChangeBoardOptionsAsync(int bookingId, int extraServiceId, int quantity)
    {
        try
        {
            string query = "UPDATE bookingextraservice " +
                           "SET extraserviceid = $2, quantity = $3 " +
                           "WHERE bookingid = $1";

            await using var cmdBoardOptions = _database.CreateCommand(query);

            cmdBoardOptions.Parameters.AddWithValue("$1", bookingId);
            cmdBoardOptions.Parameters.AddWithValue("$2", extraServiceId);
            cmdBoardOptions.Parameters.AddWithValue("$3", quantity);

            var rowsAffected = await cmdBoardOptions.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException(
                    "No rows were updated. Check if the booking exists or the inputs are valid.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating booking options: {ex.Message}");
            throw;
        }

        return bookingId;
    }

    
    
    public static async Task ChangeBookingDateAsync(int id, DateTime startdate, DateTime enddate)
    {
        try
        {
            await using var cmdInsert = _database?.CreateCommand(
                "UPDATE booking " +
                "SET startdate = $1 , enddate = $2 " +
                "WHERE id = $3 AND NOT EXISTS ( " +
                "    SELECT 1 FROM booking " +
                "    WHERE accommodationid = (SELECT accommodationid FROM booking WHERE id = $3) " +
                "    AND id != $3 " +
                "    AND (startdate <= $2 AND enddate >= $1)" +
                ");");

            cmdInsert.Parameters.AddWithValue(startdate);
            cmdInsert.Parameters.AddWithValue(enddate);
            cmdInsert.Parameters.AddWithValue(id);

            var rowsAffected = await cmdInsert.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("The new booking dates overlap with an existing booking for the same accommodation or the booking does not exist.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating booking date: {ex.Message}");
            throw;
        }
    }

    
    public static async void GetAllBookingsAsync()
    {
        await using var cmd = _database?.CreateCommand("SELECT id, customerid, accommodationid, startdate, enddate, totalprice, numberofguests  FROM booking;");
        await using var reader = await cmd?.ExecuteReaderAsync()!;

        Console.WriteLine("Bookings List:");
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int customerid = reader.GetInt32(1);
            int accommodationid = reader.GetInt32(2);
            DateTime startdate = reader.GetDateTime(3);
            DateTime enddate = reader.GetDateTime(4);
            double totalprice = reader.GetDouble(5);
            int numberofguests = reader.GetInt32(6);

            Console.WriteLine($"- ID: {id} - Customer ID: {customerid,10} - Accommodation ID: {accommodationid} - Startdate: {startdate,10} - Enddate: {enddate,10} - Total price:  {totalprice, 10} - Number of guests: {numberofguests}");
        }
    }
    
    public static async Task GetAllAddonsAsync()
    {
        await using var cmd = _database?.CreateCommand("SELECT bookingid, extraserviceid, quantity  FROM bookingextraservice;");
        await using var reader = await cmd?.ExecuteReaderAsync()!;

        Console.WriteLine("Bookings List:");
        while (reader.Read())
        {
            int bookingid = reader.GetInt32(0);
            int extraserviceid = reader.GetInt32(1);
            int quantity = reader.GetInt32(2);

            Console.WriteLine($"- Booking ID: {bookingid} - Extra service ID: {extraserviceid,10} - Quantity: {quantity}");
        }
    }
    
    
    public void ChangeBooking()
    {
        throw new NotImplementedException("ChangeBooking method is not implemented yet.");
    }


    

    public static void ChangeBoardOptionsAsync()
    {
        throw new NotImplementedException();
    }
}

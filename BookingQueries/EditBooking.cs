namespace alcocodebnb.BookingQueries;
using Npgsql;

public class EditBooking
{
    private static NpgsqlDataSource _database;

    public EditBooking(NpgsqlDataSource database)
    {
        _database = database ?? throw new ArgumentNullException(nameof(database));
    }


    public static async Task ChangeBoardOptions(int bookingId, int extraServiceId, int quantity)
    {
        try
        {
            await using var cmdBoardOptions = _database.CreateCommand(
                "UPDATE bookingextraservice" +
                "SET extraserviceid = $1, quantity = $2" +
                "WHERE bookingId = $3");
            cmdBoardOptions.Parameters.AddWithValue(extraServiceId);
            cmdBoardOptions.Parameters.AddWithValue(quantity);
            cmdBoardOptions.Parameters.AddWithValue(bookingId);

            var rowsAffected = await cmdBoardOptions.ExecuteNonQueryAsync();

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
    
    
    public static async Task ChangeBookingDateAsync(int id, DateTime startdate, DateTime enddate)
    {
        try
        {
            await using var cmdInsert = _database.CreateCommand(
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
    
    public static async void GetAllAddonsAsync()
    {
        await using var cmd = _database?.CreateCommand("SELECT bookingid, extraserviceid, quantity  FROM bookingextraservice;");
        await using var reader = await cmd?.ExecuteReaderAsync()!;

        Console.WriteLine("Bookings List:");
        while (reader.Read())
        {
            int bookingid = reader.GetInt32(0);
            int extraserviceid = reader.GetInt32(1);
            int quantity = reader.GetInt32(3);

            Console.WriteLine($"- Booking ID: {bookingid} - Extra service ID: {extraserviceid,10} - Quantity: {quantity}");
        }
    }
    
    
    public void ChangeBooking()
    {
        throw new NotImplementedException("ChangeBooking method is not implemented yet.");
    }
    
    
    

}

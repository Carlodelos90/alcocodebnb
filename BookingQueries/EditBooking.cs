namespace alcocodebnb.BookingQueries;
using Npgsql;

public class EditBooking
{
    private static NpgsqlDataSource _database;

    public EditBooking(NpgsqlDataSource database)
    {
        _database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public static async Task ChangeBoardOptionAsync(int extraserviceid, int quantity, int bookingid)
    {
        try
        {
            await using var cmdBoardOption = _database.CreateCommand(
                "UPDATE bookingextraservice " +
                "SET extraserviceid = @ExtraServiceId, quantity = @Quantity " +
                "WHERE bookingid = @BookingId");

            cmdBoardOption.Parameters.AddWithValue("@ExtraServiceId", extraserviceid);
            cmdBoardOption.Parameters.AddWithValue("@Quantity", quantity);
            cmdBoardOption.Parameters.AddWithValue("@BookingId", bookingid);

            await cmdBoardOption.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating board option: {ex.Message}");
            throw;
        }
    }

    public static async Task ChangeGuestsAsync(int id, int numberofguests)
    {
        try
        {
            await using var cmdGuests = _database.CreateCommand(
                "UPDATE booking SET numberofguests = @numberofguests WHERE id = @Id");

            cmdGuests.Parameters.AddWithValue("@numberofguests", numberofguests);
            cmdGuests.Parameters.AddWithValue("@Id", id);

            await cmdGuests.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating guests: {ex.Message}");
            throw;
        }
    }

    public static async Task ChangeBookingDateAsync(int id, DateTime startDate, DateTime endDate)
    {
        try
        {
            await using var cmdBooking = _database.CreateCommand(
                "UPDATE booking " +
                "SET startdate = @StartDate, enddate = @EndDate " +
                "WHERE id = @Id");

            cmdBooking.Parameters.AddWithValue("@StartDate", startDate);
            cmdBooking.Parameters.AddWithValue("@EndDate", endDate);
            cmdBooking.Parameters.AddWithValue("@Id", id);

            await cmdBooking.ExecuteNonQueryAsync();
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

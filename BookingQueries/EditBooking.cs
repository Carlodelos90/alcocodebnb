namespace alcocodebnb.BookingQueries;
using Npgsql;

public class EditBooking
{
    private static NpgsqlDataSource _database;

    public EditBooking(NpgsqlDataSource database)
    {
        _database = database;
    }

    public static async void ChangeBoardOption(int extraserviceid, int quantity, int bookingid)
    {
        await using (var cmdBoardOption = _database.CreateCommand("UPDATE bookingextraservice " + 
                                                                  "SET extraserviceid = $1, quantity = $2 " +
                                                                  "WHERE bookingid = $3 "))
        {
            cmdBoardOption.Parameters.AddWithValue("$1", extraserviceid);
            cmdBoardOption.Parameters.AddWithValue("$2", quantity);
            cmdBoardOption.Parameters.AddWithValue("$3", bookingid);
            await cmdBoardOption.ExecuteNonQueryAsync();
        }
    }

    public async void ChangeGuests(int id, int numberofguests)
    {
        await using (var cmdGuests = _database.CreateCommand("UPDATE booking SET guests = $1 " +
                                                             "WHERE id = $2"))
        {
            cmdGuests.Parameters.AddWithValue(numberofguests);
            await cmdGuests.ExecuteNonQueryAsync();
        }
    }
    public static async void ChangeBookingDate(int id, DateTime startdate, DateTime enddate)
    {
        await using (var cmdBooking = _database.CreateCommand("UPDATE booking " +
                                                       "SET startdate = $1, enddate = $2" +
                                                       " WHERE id = $3"))
        {
            cmdBooking.Parameters.AddWithValue(startdate);
            cmdBooking.Parameters.AddWithValue(enddate);
            cmdBooking.Parameters.AddWithValue(id);
            await cmdBooking.ExecuteNonQueryAsync();
        }
        
        
    }

    public static void ChangeBooking()
    {
        throw new NotImplementedException();
    }
}
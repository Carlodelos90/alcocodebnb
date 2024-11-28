namespace alcocodebnb.BookingQueries;
using Npgsql;

public class CancelBooking
{
    
    private static NpgsqlDataSource _database;

    public CancelBooking(NpgsqlDataSource database)
    {
        _database = database;
    }
    
    public static async void AllBookings()
    {
        await using (var cmd = _database.CreateCommand("SELECT id FROM booking")) // Skapa vårt kommand/query
        await using (var reader = await cmd.ExecuteReaderAsync()) // Kör vår kommando/query och inväntar resultatet.
            while ( await reader.ReadAsync()) // Läser av 1 rad/objekt i taget ifrån resultatet och kommer avsluta loopen när det inte finns fler rader att läsa. 
            {
                Console.WriteLine($"Id: {reader.GetInt32(0)}");
                                 
            }
    }
    
    public static async void DeleteBooking(int id)
    {
        await using (var cmd = _database.CreateCommand("DELETE FROM booking WHERE id = $1"))
        {
            cmd.Parameters.AddWithValue(id);
            int result = await cmd.ExecuteNonQueryAsync();
            Console.WriteLine(result);
        }
    }

    public static void DeleteBooking()
    {
        throw new NotImplementedException();
    }
}
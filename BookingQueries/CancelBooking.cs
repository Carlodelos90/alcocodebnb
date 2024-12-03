using Npgsql;

namespace alcocodebnb.BookingQueries
{
    public class CancelBooking
    {
    
        private static NpgsqlDataSource? _database;

        public CancelBooking(NpgsqlDataSource database)
        {
            _database = database;
        }
    
        public static async void AllBookings()
        {
            await using var cmd = _database?.CreateCommand("SELECT id FROM booking");
            await using var reader = await cmd?.ExecuteReaderAsync()!;
            while ( await reader.ReadAsync())
            {
                Console.WriteLine($"Id: {reader.GetInt32(0)}");
                                 
            }
        }
    
        public static async void DeleteBooking(int id)
        {
            await using var cmd = _database?.CreateCommand("DELETE FROM booking WHERE id = $1");
            cmd?.Parameters.AddWithValue(id);
            int result = await cmd?.ExecuteNonQueryAsync()!;
            Console.WriteLine(result);
        }

        public static void DeleteBooking()
        {
            throw new NotImplementedException();
        }
    }
}

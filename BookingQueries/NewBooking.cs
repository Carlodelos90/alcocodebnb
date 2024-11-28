namespace alcocodebnb.BookingQueries;
using Npgsql;

public class NewBooking
    {
        private static NpgsqlDataSource _database;

        public NewBooking(NpgsqlDataSource database)
        {
            _database = database;
        }
        
        
        public static async void AddNewBooking(int accommodationid, DateOnly startdate, DateOnly enddate, float totalprice, int numberofguests)
        {
            await using 
                (var cmd = _database.CreateCommand("INSERT INTO booking " +
                                                               "(accommodationid, startdate, enddate, totalprice, numberofguests) " +
                                                           "VALUES ($1, $2, $3, $4, $5)"))
            {
                cmd.Parameters.AddWithValue(accommodationid);
                cmd.Parameters.AddWithValue(startdate);
                cmd.Parameters.AddWithValue(enddate);
                cmd.Parameters.AddWithValue(totalprice);
                cmd.Parameters.AddWithValue(numberofguests);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public static void AddNewBooking()
        {
            throw new NotImplementedException();
        }
        
        public static async void AllLocations()
        {
            await using (var cmd = _database.CreateCommand("SELECT * FROM location ORDER BY id ASC")) // Skapa vårt kommand/query
            await using (var reader = await cmd.ExecuteReaderAsync()) // Kör vår kommando/query och inväntar resultatet.
                while ( await reader.ReadAsync()) // Läser av 1 rad/objekt i taget ifrån resultatet och kommer avsluta loopen när det inte finns fler rader att läsa. 
                {
                    Console.WriteLine($"id: {reader.GetInt32(0)}, " +
                                      $"name: {reader.GetString(1)}, " +
                                      $"country: {reader.GetString(2)}, " +
                                      $"region: {reader.GetString(3)}");


                }
        }
    }


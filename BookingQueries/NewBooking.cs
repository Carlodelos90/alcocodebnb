using System.Data;

namespace alcocodebnb.BookingQueries;
using Npgsql;

public class NewBooking
    {
        private static NpgsqlDataSource _database;

        public NewBooking(NpgsqlDataSource database)
        {
            _database = database;
        }
        
        
        
        
        public static async void AddNewBooking(int customerId, int accommodationId, DateOnly startDateTime, DateOnly endDateTime, float totalPrice, int numberOfGuests, string status)
        {
            await using var cmd = _database.CreateCommand("INSERT INTO booking customerid, accommodationid, startdate, enddate, totalprice, numberofguests, status) VALUES ($1, $2, $3, $4, $5, $6, $7)");
            cmd.Parameters.AddWithValue(customerId);
            cmd.Parameters.AddWithValue(accommodationId);
            cmd.Parameters.AddWithValue(startDateTime);
            cmd.Parameters.AddWithValue(endDateTime);
            cmd.Parameters.AddWithValue(totalPrice);
            cmd.Parameters.AddWithValue(numberOfGuests);
            cmd.Parameters.AddWithValue(status);
            await cmd.ExecuteNonQueryAsync();
        }

        public static void AddNewBooking()
        {
            throw new NotImplementedException();
        }
        
        public static async void AllLocations()
        {
            await using var cmd = _database.CreateCommand("SELECT * FROM location ORDER BY id ASC");
            await using var reader = await cmd.ExecuteReaderAsync();
            while ( await reader.ReadAsync()) 
            {
                Console.WriteLine($"id: {reader.GetInt32(0)}, " +
                                  $"name: {reader.GetString(1)}, " +
                                  $"country: {reader.GetString(2)}, " +
                                  $"region: {reader.GetString(3)}");


            }
        }
        
        
        public static async void ShowAccommodations(int chosenId)
        {
            string query = "SELECT id, name, baseprice, rating, pool, eveningentertainment, kidsclub, restaurant FROM accommodation WHERE location = $1;";

            await using var cmd = _database.CreateCommand(query);
            cmd.Parameters.AddWithValue(chosenId);
            await using var reader = await cmd.ExecuteReaderAsync();

            Console.WriteLine("\nAvailable Accommodations:");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"{"ID",-5} {"Name",-30} {"Rating",-7} {"Price",-10} {"Pool",-5} {"Evening Ent.",-15} {"Kids Club",-10} {"Restaurant",-10}");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------");

            while (await reader.ReadAsync())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                double baseprice = reader.GetDouble(2);
                double rating = reader.IsDBNull(3) ? 0.0 : reader.GetDouble(3);
                bool pool = reader.GetBoolean(4);
                bool eveningEntertainment = reader.GetBoolean(5);
                bool kidsClub = reader.GetBoolean(6);
                bool restaurant = reader.GetBoolean(7);

                Console.WriteLine(
                    $"{id,-5} " +
                    $"Hotel: {name,-50} " +
                    $"Price: {baseprice,5}$ " +
                    $"Rating: {rating,7:N1} " +
                    $"Pool:{(pool ? "Yes" : "No"),-5} " +
                    $"Evening shows: {(eveningEntertainment ? "Yes" : "No"),-15} " +
                    $"Kids club: {(kidsClub ? "Yes" : "No"),-10} " +
                    $"Restaurant: {(restaurant ? "Yes" : "No"),-10}");
            }

            Console.WriteLine("---------------------------------------------------------------------------------------------------------");
        }
    }


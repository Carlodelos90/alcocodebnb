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
        
        
        
        
        public static async void AddNewBooking(int customerId, int accommodationId, DateTime startDateTime, DateTime endDateTime, decimal totalPrice, int numberOfGuests)
        {
            await using var cmd = _database.CreateCommand("INSERT INTO booking (customerid, accommodationid, startdate, enddate, totalprice, numberofguests) VALUES ($1, $2, $3, $4, $5, $6) RETURNING id;");
            cmd.Parameters.AddWithValue(customerId);
            cmd.Parameters.AddWithValue(accommodationId);
            cmd.Parameters.AddWithValue(startDateTime);
            cmd.Parameters.AddWithValue(endDateTime);
            cmd.Parameters.AddWithValue(totalPrice);
            cmd.Parameters.AddWithValue(numberOfGuests);
            await cmd.ExecuteNonQueryAsync();
            
            var newBookingId = (int)await cmd.ExecuteScalarAsync();
            
            if (newBookingId > 0)
            {
                Console.WriteLine("\nBooking added successfully.");
                // Display the booking summary
                ShowBookingSummary(newBookingId);
            }
            else
            {
                Console.WriteLine("Failed to add booking.");
            }
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
            Console.WriteLine($"{"ID",-5} {"Name",-50}");
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
                    $"Hotel: {name,-50} |" +
                    $"Price: {baseprice,5}$ |" +
                    $"Rating: {rating,7:N1} |" +
                    $"Pool:{(pool ? "Yes" : "No"),-5} |" +
                    $"Evening shows: {(eveningEntertainment ? "Yes" : "No"),-15} |" +
                    $"Kids club: {(kidsClub ? "Yes" : "No"),-10} |" +
                    $"Restaurant: {(restaurant ? "Yes" : "No"),-10} |");
            }

            Console.WriteLine("---------------------------------------------------------------------------------------------------------");
        }
        
        
        
        public static async void ShowBookingSummary(int bookingId)
        {
            string query = @"
                SELECT 
                    b.id AS BookingID,
                    c.firstname || ' ' || c.lastname AS CustomerName,
                    a.name AS AccommodationName,
                    b.startdate,
                    b.enddate,
                    b.totalprice,
                    b.numberofguests
                FROM booking b
                JOIN customer c ON b.customerid = c.id
                JOIN accommodation a ON b.accommodationid = a.id
                WHERE b.id = $1;";

            try
            {
                await using var cmd = _database.CreateCommand(query);
                cmd.Parameters.AddWithValue(bookingId);

                await using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    int id = reader.GetInt32(0);
                    string customerName = reader.GetString(1);
                    string accommodationName = reader.GetString(2);
                    DateTime startDate = reader.GetDateTime(3);
                    DateTime endDate = reader.GetDateTime(4);
                    decimal totalPrice = reader.GetDecimal(5);
                    int numberOfGuests = reader.GetInt32(6);

                    Console.WriteLine("\nBooking Summary:");
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine($"Booking ID       : {id}");
                    Console.WriteLine($"Customer Name    : {customerName}");
                    Console.WriteLine($"Accommodation    : {accommodationName}");
                    Console.WriteLine($"Start Date       : {startDate.ToShortDateString()}");
                    Console.WriteLine($"End Date         : {endDate.ToShortDateString()}");
                    Console.WriteLine($"Total Price      : {totalPrice:C}");
                    Console.WriteLine($"Number of Guests : {numberOfGuests}");
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine("The invoice was sent to the customer's email address and the accommodation's email address.");
                    Console.WriteLine("--------------------------------------------------");
                    
                }
                else
                {
                    Console.WriteLine("Booking not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving booking summary: {ex.Message}");
            }
        }

        /*public static void AddNewBooking(int customerId, int accommodationId, DateTime startDateTime, DateTime endDateTime, decimal totalPrice, int numberOfGuests)
        {
            throw new NotImplementedException();
        }*/
    }


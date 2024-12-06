#region Using Directives
using Npgsql;
#endregion

namespace alcocodebnb.BookingQueries;

public class NewBooking
{
    private static NpgsqlDataSource? _database;
    public NewBooking(NpgsqlDataSource database)
    {
        _database = database;
    }

    #region Add New Booking
    public static async Task AddNewBookingAsync(int customerId, int accommodationId, DateTime startDateTime,
        DateTime endDateTime, int numberOfGuests)
    {
        if (_database == null)
        {
            throw new InvalidOperationException("Database connection is not initialized.");
        }

        try
        {
            decimal basePrice = await GetBasePriceAsync(accommodationId);
            int numberOfDays = CalculateNumberOfDays(startDateTime, endDateTime);
            decimal totalPrice = basePrice * numberOfDays;

            string query = @"
                INSERT INTO booking (customerid, accommodationid, startdate, enddate, totalprice, numberofguests)
                VALUES (@CustomerId, @AccommodationId, @StartDate, @EndDate, @TotalPrice, @NumberOfGuests)
                RETURNING id;
            ";

            await using var cmd = _database.CreateCommand(query);
            cmd.Parameters.AddWithValue("CustomerId", customerId);
            cmd.Parameters.AddWithValue("AccommodationId", accommodationId);
            cmd.Parameters.AddWithValue("StartDate", startDateTime);
            cmd.Parameters.AddWithValue("EndDate", endDateTime);
            cmd.Parameters.AddWithValue("TotalPrice", totalPrice);
            cmd.Parameters.AddWithValue("NumberOfGuests", numberOfGuests);

            object? result = await cmd.ExecuteScalarAsync();

            if (result != null && int.TryParse(result.ToString(), out int newBookingId))
            {
                if (newBookingId > 0)
                {
                    Console.WriteLine("\nBooking added successfully.");
                    await ShowBookingSummaryAsync(newBookingId);
                }
                else
                {
                    Console.WriteLine("Failed to add booking.");
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve the new booking ID.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating booking: {ex.Message}");
        }
    }
    #endregion

    #region Helper Methods
    private static async Task<decimal> GetBasePriceAsync(int accommodationId)
    {
        if (_database == null)
        {
            throw new InvalidOperationException("Database connection is not initialized.");
        }

        string query = "SELECT baseprice FROM accommodation WHERE id = @AccommodationId;";

        try
        {
            await using var cmd = _database.CreateCommand(query);
            cmd.Parameters.AddWithValue("AccommodationId", accommodationId);

            object? result = await cmd.ExecuteScalarAsync();

            if (result != null && decimal.TryParse(result.ToString(), out decimal basePrice))
            {
                return basePrice;
            }
            else
            {
                throw new InvalidOperationException("Base price not found for the selected accommodation.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching base price: {ex.Message}");
            throw;
        }
    }

    private static int CalculateNumberOfDays(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
        {
            throw new ArgumentException("End date must be after start date.");
        }

        return (endDate.Date - startDate.Date).Days;
    }
    #endregion


    public async Task AllLocationsAsync()
    {
        if (_database == null)
        {
            Console.WriteLine("Database connection is not initialized.");
            return;
        }

        string query = "SELECT * FROM location;";

        try
        {
            await using var cmd = _database.CreateCommand(query);
            await using var reader = await cmd.ExecuteReaderAsync();

            Console.WriteLine("\nAvailable Locations:");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{"ID",-5} {"Name",-30} {"Country",-20} {"Region",-20}");
            Console.WriteLine("--------------------------------------------------------------");

            while (await reader.ReadAsync())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string country = reader.GetString(2);
                string region = reader.GetString(3);

                Console.WriteLine($"{id,-5} {name,-30} {country,-20} {region,-20}");
            }

            Console.WriteLine("--------------------------------------------------------------");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching locations: {ex.Message}");
        }
    }

    public async Task ShowAccommodationsAsync(int chosenId)
    {
        if (_database == null)
        {
            Console.WriteLine("Database connection is not initialized.");
            return;
        }

        string query = @"
            SELECT id, name, baseprice, rating, pool, eveningentertainment, kidsclub, restaurant 
            FROM accommodation 
            WHERE location = @LocationId;
        ";

        try
        {
            await using var cmd = _database.CreateCommand(query);
            cmd.Parameters.AddWithValue("LocationId", chosenId);
            await using var reader = await cmd.ExecuteReaderAsync();

            Console.WriteLine("\nAvailable Accommodations:");
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine($"{"ID",-5} {"Name",-40} {"BasePrice",-10} {"Rating",-7} {"Pool",-6} {"Shows",-18} {"Kids Club",-12} {"Restaurant",-12}");
            Console.WriteLine("---------------------------------------------------------------------------------------------");

            while (await reader.ReadAsync())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal basePrice = reader.GetDecimal(2);
                double rating = reader.IsDBNull(3) ? 0.0 : reader.GetDouble(3);
                bool pool = reader.GetBoolean(4);
                bool eveningEntertainment = reader.GetBoolean(5);
                bool kidsClub = reader.GetBoolean(6);
                bool restaurant = reader.GetBoolean(7);

                Console.WriteLine(
                    $"{id,-5} " +
                    $"{name,-40} " +
                    $"{basePrice, -10:C} " +
                    $"{rating, -7:N1} " +
                    $"{(pool ? "Yes" : "No"),-6} " +
                    $"{(eveningEntertainment ? "Yes" : "No"),-18} " +
                    $"{(kidsClub ? "Yes" : "No"),-12} " +
                    $"{(restaurant ? "Yes" : "No"),-12}");
            }

            Console.WriteLine("---------------------------------------------------------------------------------------------");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching accommodations: {ex.Message}");
        }
    }

    public static async Task ShowBookingSummaryAsync(int bookingId)
    {
        if (_database == null)
        {
            Console.WriteLine("Database connection is not initialized.");
            return;
        }

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
            WHERE b.id = @BookingId;
        ";

        try
        {
            await using var cmd = _database.CreateCommand(query);
            cmd.Parameters.AddWithValue("BookingId", bookingId);

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
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine($"Booking ID       : {id}");
                Console.WriteLine($"Customer Name    : {customerName}");
                Console.WriteLine($"Accommodation    : {accommodationName}");
                Console.WriteLine($"Start Date       : {startDate.ToShortDateString()}");
                Console.WriteLine($"End Date         : {endDate.ToShortDateString()}");
                Console.WriteLine($"Total Price      : {totalPrice:C}");
                Console.WriteLine($"Number of Guests : {numberOfGuests}");
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine("The invoice was sent to the customer's email address and the accommodation's email address.");
                Console.WriteLine("--------------------------------------------------------------");
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

    public static async Task FilterPriceAscAsync()
    {
        if (_database == null)
        {
            Console.WriteLine("Database connection is not initialized.");
            return;
        }

        string query = @"
            SELECT id, name, baseprice, rating, pool, eveningentertainment, kidsclub, restaurant 
            FROM accommodation 
            ORDER BY baseprice ASC;
        ";

        try
        {
            await using var cmd = _database.CreateCommand(query);
            await using var reader = await cmd.ExecuteReaderAsync();

            Console.WriteLine("\nAccommodations Sorted by Price (Ascending):");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{"ID",-5} {"Name",-40} {"Price",-12}");
            Console.WriteLine("--------------------------------------------------------------");

            while (await reader.ReadAsync())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal basePrice = reader.GetDecimal(2);

                Console.WriteLine(
                    $"{id,-5} " +
                    $"{name,-40} " +
                    $"{basePrice, -12:C}");
            }

            Console.WriteLine("--------------------------------------------------------------");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sorting accommodations by price ascending: {ex.Message}");
        }
    }

    public static async Task FilterPriceDescAsync()
    {
        if (_database == null)
        {
            Console.WriteLine("Database connection is not initialized.");
            return;
        }

        string query = @"
            SELECT id, name, baseprice, rating, pool, eveningentertainment, kidsclub, restaurant 
            FROM accommodation 
            ORDER BY baseprice DESC;
        ";

        try
        {
            await using var cmd = _database.CreateCommand(query);
            await using var reader = await cmd.ExecuteReaderAsync();

            Console.WriteLine("\nAccommodations Sorted by Price (Descending):");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{"ID",-5} {"Name",-40} {"Price",-12}");
            Console.WriteLine("--------------------------------------------------------------");

            while (await reader.ReadAsync())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal basePrice = reader.GetDecimal(2);

                Console.WriteLine(
                    $"{id,-5} " +
                    $"{name,-40} " +
                    $"{basePrice, -12:C}");
            }

            Console.WriteLine("--------------------------------------------------------------");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sorting accommodations by price descending: {ex.Message}");
        }
    }

    public static async Task FilterReviewAsync()
    {
        if (_database == null)
        {
            Console.WriteLine("Database connection is not initialized.");
            return;
        }

        string query = @"
            SELECT name, rating 
            FROM accommodation 
            ORDER BY rating DESC;
        ";

        try
        {
            await using var cmd = _database.CreateCommand(query);
            await using var reader = await cmd.ExecuteReaderAsync();

            Console.WriteLine("\nAccommodations Sorted by Rating (Descending):");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{"Name",-40} {"Rating",-10}");
            Console.WriteLine("--------------------------------------------------------------");

            while (await reader.ReadAsync())
            {
                string name = reader.GetString(0);
                double rating = reader.IsDBNull(1) ? 0.0 : reader.GetDouble(1);

                Console.WriteLine($"{name,-40} {rating, -10:N1}");
            }

            Console.WriteLine("--------------------------------------------------------------");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error filtering accommodations by review: {ex.Message}");
        }
    }
    
    #region Add Extra services
    public static async Task AddExtraService(int bookingid, int extraserviceid, int quantity)
    {
        if (_database == null)
        {
            throw new InvalidOperationException("Database connection is not initialized.");
        }

        try
        {
            string query = @"
                INSERT INTO bookingextraservice (bookingid, extraserviceid, quantity) VALUES (@bookingid, @extraserviceid, @quantity);
            ";

            await using var cmd = _database.CreateCommand(query);
            cmd.Parameters.AddWithValue("bookingid", bookingid);
            cmd.Parameters.AddWithValue("extraserviceid", extraserviceid);
            cmd.Parameters.AddWithValue("quantity", quantity);

            await cmd.ExecuteScalarAsync();
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating booking: {ex.Message}");
        }
    }
    #endregion
    
    
    
    #region SortByDistanceToCenter

    public static async Task SortByDistanceToCenterAsync()
    {
        if (_database == null)
        {
            Console.WriteLine("Database connection is not initialized.");
            return;
        }

        string query = @"
            SELECT id, name, distancetobeach, distancetocenter, baseprice, rating, pool, eveningentertainment, kidsclub, restaurant 
            FROM accommodation 
            ORDER BY distancetocenter ASC;
        ";

        try
        {
            await using var cmd = _database.CreateCommand(query);
            await using var reader = await cmd.ExecuteReaderAsync();

            Console.WriteLine("\nAccommodations Sorted by Distance to Center (Ascending):");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine($"{"ID",-5} {"Name",-40} {"Price",-12} {"DistanceToCenter",-18}");
            Console.WriteLine("--------------------------------------------------------------------------------");

            while (await reader.ReadAsync())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal basePrice = reader.GetDecimal(4);
                decimal distanceToCenter = reader.GetDecimal(3);

                Console.WriteLine(
                    $"{id,-5} " +
                    $"{name,-40} " +
                    $"{basePrice, -12:C} " +
                    $"{distanceToCenter, -18:F1}");
            }

            Console.WriteLine("--------------------------------------------------------------------------------");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sorting accommodations by distance to center ascending: {ex.Message}");
        }
    }
    #endregion
    
    #region SortByDistanceToBeach
    
    public static async Task SortByDistanceToBeach()
    {
        if (_database == null)
        {
            Console.WriteLine("Database connection is not initialized.");
            return;
        }

        string query = @"
            SELECT name, distancetobeach 
            FROM accommodation 
            ORDER BY distancetobeach DESC;
        ";

        try
        {
            await using var cmd = _database.CreateCommand(query);
            await using var reader = await cmd.ExecuteReaderAsync();

            Console.WriteLine("\nAccommodations Sorted by Distance to Beach (Descending):");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine($"{"Name",-40} {"DistanceToBeach",-15}");
            Console.WriteLine("--------------------------------------------------------------");

            while (await reader.ReadAsync())
            {
                string name = reader.GetString(0);
                double distancetobeach = reader.IsDBNull(1) ? 0.0 : reader.GetDouble(1);

                Console.WriteLine($"{name,-40} {distancetobeach, -15:N1}");
            }

            Console.WriteLine("--------------------------------------------------------------");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error filtering distance to beach: {ex.Message}");
        }
    }
    #endregion
}
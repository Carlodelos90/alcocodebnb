#region Using Directives
using Npgsql;
#endregion

namespace alcocodebnb.AccommodationQueries;

public class AccommodationManager
{
    private readonly NpgsqlDataSource _database;

    public AccommodationManager(NpgsqlDataSource database)
    {
        _database = database;
    }

    #region Search Available Accommodations
    public async Task SearchAvailableAccommodationsAsync(DateTime startDate, DateTime endDate, int? locationId = null, decimal? minPrice = null, decimal? maxPrice = null)
    {
        if (endDate <= startDate)
        {
            Console.WriteLine("End date must be after start date.");
            return;
        }

        string query = @"
            SELECT a.id, a.name, a.baseprice, a.rating, a.pool, a.eveningentertainment, a.kidsclub, a.restaurant
            FROM accommodation a
            WHERE
                (@LocationId IS NULL OR a.location = @LocationId) AND
                (@MinPrice IS NULL OR a.baseprice >= @MinPrice) AND
                (@MaxPrice IS NULL OR a.baseprice <= @MaxPrice) AND
                a.id NOT IN (
                    SELECT b.accommodationid
                    FROM booking b
                    WHERE b.startdate <= @EndDate AND b.enddate >= @StartDate
                )
            ORDER BY a.id;";

        try
        {
            await using var cmd = _database.CreateCommand(query);

            cmd.Parameters.AddWithValue("StartDate", startDate);
            cmd.Parameters.AddWithValue("EndDate", endDate);
            cmd.Parameters.AddWithValue("LocationId", locationId.HasValue ? (object)locationId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("MinPrice", minPrice.HasValue ? (object)minPrice.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("MaxPrice", maxPrice.HasValue ? (object)maxPrice.Value : DBNull.Value);

            await using var reader = await cmd.ExecuteReaderAsync();

            Console.WriteLine("\nAvailable Accommodations:");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"{"ID",-5} {"Name",-50}");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------");

            while (await reader.ReadAsync())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal baseprice = reader.GetDecimal(2);
                double rating = reader.IsDBNull(3) ? 0.0 : reader.GetDouble(3);
                bool pool = reader.GetBoolean(4);
                bool eveningEntertainment = reader.GetBoolean(5);
                bool kidsClub = reader.GetBoolean(6);
                bool restaurant = reader.GetBoolean(7);

                Console.WriteLine(
                    $"{id,-5} " +
                    $"{name,-50} " +
                    $"{baseprice,10:C} " +
                    $"{rating,7:N1} " +
                    $"{(pool ? "Yes" : "No"),-5} " +
                    $"{(eveningEntertainment ? "Yes" : "No"),-15} " +
                    $"{(kidsClub ? "Yes" : "No"),-10} " +
                    $"{(restaurant ? "Yes" : "No"),-10}");
            }

            Console.WriteLine("---------------------------------------------------------------------------------------------------------");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching accommodations: {ex.Message}");
        }
    }
    #endregion
}
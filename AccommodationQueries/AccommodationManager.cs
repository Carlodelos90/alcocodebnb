#region Using Directives

using Npgsql;
using NpgsqlTypes;

#endregion

namespace alcocodebnb.AccommodationQueries
{
    public class AccommodationManager
    {
        private readonly NpgsqlDataSource _database;

        public AccommodationManager(NpgsqlDataSource database)
        {
            _database = database;
        }

        #region Search Available Accommodations
            public async Task SearchAvailableAccommodationsAsync(
                DateTime startDate,
                DateTime endDate,
                int? locationId = null,
                decimal? minPrice = null,
                decimal? maxPrice = null,
                decimal? maxDistanceToBeach = null,
                decimal? maxDistanceToCenter = null,
                bool? pool = null,
                bool? eveningEntertainment = null,
                bool? kidsClub = null,
                bool? restaurant = null
            )
            {
                if (endDate <= startDate)
                {
                    Console.WriteLine("End date must be after start date.");
                    return;
                }

                string query = @"
                    SELECT a.id, a.name, a.distancetobeach, a.distancetocenter, a.baseprice, a.rating, a.pool, a.eveningentertainment, a.kidsclub, a.restaurant
                    FROM accommodation a
                    WHERE
                        (@LocationId IS NULL OR a.location = @LocationId) AND
                        (@MinPrice IS NULL OR a.baseprice >= @MinPrice) AND
                        (@MaxPrice IS NULL OR a.baseprice <= @MaxPrice) AND
                        (@MaxDistanceToBeach IS NULL OR a.distancetobeach <= @MaxDistanceToBeach) AND
                        (@MaxDistanceToCenter IS NULL OR a.distancetocenter <= @MaxDistanceToCenter) AND
                        (@Pool IS NULL OR a.pool = @Pool) AND
                        (@EveningEntertainment IS NULL OR a.eveningentertainment = @EveningEntertainment) AND
                        (@KidsClub IS NULL OR a.kidsclub = @KidsClub) AND
                        (@Restaurant IS NULL OR a.restaurant = @Restaurant) AND

                        
                        a.id NOT IN (
                            SELECT b.accommodationid
                            FROM booking b
                            WHERE b.startdate <= @EndDate AND b.enddate >= @StartDate
                        )
                    ORDER BY a.id;";
                

                try
                {
                    await using var cmd = _database.CreateCommand(query);
                    
                    cmd.Parameters.Add("@StartDate", NpgsqlDbType.Date).Value = startDate;
                    cmd.Parameters.Add("@EndDate", NpgsqlDbType.Date).Value = endDate;
                    cmd.Parameters.Add("@LocationId", NpgsqlDbType.Integer).Value = locationId.HasValue ? locationId.Value : DBNull.Value;
                    cmd.Parameters.Add("@MinPrice", NpgsqlDbType.Numeric).Value = minPrice.HasValue ? minPrice.Value : DBNull.Value;
                    cmd.Parameters.Add("@MaxPrice", NpgsqlDbType.Numeric).Value = maxPrice.HasValue ? maxPrice.Value : DBNull.Value;
                    
                    cmd.Parameters.Add("@MaxDistanceToBeach", NpgsqlDbType.Numeric).Value = maxDistanceToBeach.HasValue ? maxDistanceToBeach.Value : DBNull.Value;
                    cmd.Parameters.Add("@MaxDistanceToCenter", NpgsqlDbType.Numeric).Value = maxDistanceToCenter.HasValue ? maxDistanceToCenter.Value : DBNull.Value;
                    
                    cmd.Parameters.Add("@Pool", NpgsqlDbType.Boolean).Value = pool.HasValue ? pool.Value : DBNull.Value;
                    cmd.Parameters.Add("@EveningEntertainment", NpgsqlDbType.Boolean).Value = eveningEntertainment.HasValue ? eveningEntertainment.Value : DBNull.Value;
                    cmd.Parameters.Add("@KidsClub", NpgsqlDbType.Boolean).Value = kidsClub.HasValue ? kidsClub.Value : DBNull.Value;
                    cmd.Parameters.Add("@Restaurant", NpgsqlDbType.Boolean).Value = restaurant.HasValue ? restaurant.Value : DBNull.Value;

                    await using var reader = await cmd.ExecuteReaderAsync();

                    Console.WriteLine("\nAvailable Accommodations:");
                    Console.WriteLine(new string('-', 140));
                    Console.WriteLine(
                        $"{"ID",-5}{"Name",-50}{"Price",-15}{"Beach Distance",-20}{"Center Distance",-20}{"Rating",-10}{"Pool",-5}{"Evening Ent.",-15}{"Kids Club",-10}{"Restaurant",-10}");
                    Console.WriteLine(new string('-', 140));

                    bool anyAvailable = false;

                    while (await reader.ReadAsync())
                    {
                        anyAvailable = true;

                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        decimal distancetobeach = reader.GetDecimal(2);
                        decimal distancetocenter = reader.GetDecimal(3);
                        decimal baseprice = reader.GetDecimal(4);
                        double rating = reader.IsDBNull(5) ? 0.0 : reader.GetDouble(5);
                        bool poolValue = reader.GetBoolean(6);
                        bool eveningEntertainmentValue = reader.GetBoolean(7);
                        bool kidsClubValue = reader.GetBoolean(8);
                        bool restaurantValue = reader.GetBoolean(9);

                        Console.WriteLine(
                            $"{id,-5}" +
                            $"{name,-50}" +
                            $"{($"{baseprice:N2} USD"),-15}" +
                            $"{distancetobeach,-20:N1}" +
                            $"{distancetocenter,-20:N1}" +
                            $"{rating,-10:N1}" +
                            $"{(poolValue ? "Yes" : "No"),-5}" +
                            $"{(eveningEntertainmentValue ? "Yes" : "No"),-15}" +
                            $"{(kidsClubValue ? "Yes" : "No"),-10}" +
                            $"{(restaurantValue ? "Yes" : "No"),-10}");
                    }
                    if (!anyAvailable)
                    {
                        Console.WriteLine("No available accommodations found for the selected criteria.");
                    }

                    Console.WriteLine("---------------------------------------------------------------------------------------------------------");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error searching accommodations: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                }
            }
        #endregion
    }
}
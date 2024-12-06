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
                    -- sökkriterier för filtret i postgres
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
                cmd.Parameters.Add("@LocationId", NpgsqlDbType.Integer).Value = locationId.HasValue ? (object)locationId.Value : DBNull.Value;
                cmd.Parameters.Add("@MinPrice", NpgsqlDbType.Numeric).Value = minPrice.HasValue ? (object)minPrice.Value : DBNull.Value;
                cmd.Parameters.Add("@MaxPrice", NpgsqlDbType.Numeric).Value = maxPrice.HasValue ? (object)maxPrice.Value : DBNull.Value;
                
                cmd.Parameters.Add("@MaxDistanceToBeach", NpgsqlDbType.Numeric).Value = maxDistanceToBeach.HasValue ? (object)maxDistanceToBeach.Value : DBNull.Value;
                cmd.Parameters.Add("@MaxDistanceToCenter", NpgsqlDbType.Numeric).Value = maxDistanceToCenter.HasValue ? (object)maxDistanceToCenter.Value : DBNull.Value;
                
                cmd.Parameters.Add("@Pool", NpgsqlDbType.Boolean).Value = pool.HasValue ? (object)pool.Value : DBNull.Value;
                cmd.Parameters.Add("@EveningEntertainment", NpgsqlDbType.Boolean).Value = eveningEntertainment.HasValue ? (object)eveningEntertainment.Value : DBNull.Value;
                cmd.Parameters.Add("@KidsClub", NpgsqlDbType.Boolean).Value = kidsClub.HasValue ? (object)kidsClub.Value : DBNull.Value;
                cmd.Parameters.Add("@Restaurant", NpgsqlDbType.Boolean).Value = restaurant.HasValue ? (object)restaurant.Value : DBNull.Value;

                await using var reader = await cmd.ExecuteReaderAsync();

                Console.WriteLine("\nAvailable Accommodations:");
                Console.WriteLine(new string('-', 150)); 

// vänsterjustering för att det är lättare att förhålla sig till
                Console.WriteLine(
                    $"{"ID",-10}" +
                    $"{"Name",-60}" +
                    $"{"Price",-15}" +
                    $"{"Beach Dist",-20}" +
                    $"{"Center Dist",-20}" +
                    $"{"Rating",-10}" +
                    $"{"Pool",-10}" +
                    $"{"Evening Ent.",-20}" +
                    $"{"Kids Club",-15}" +
                    $"{"Restaurant",-15}"
                );
                Console.WriteLine(new string('-', 200)); 

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
                        $"{id,-10}" +
                        $"{name,-60}" +
                        $"{baseprice,-15:C}" +
                        $"{distancetobeach,-20:N1} km" +
                        $"{distancetocenter,-20:N1} km" +
                        $"{rating,-10:N1}" +
                        $"{(poolValue ? "Yes" : "No"),-10}" +
                        $"{(eveningEntertainmentValue ? "Yes" : "No"),-20}" +
                        $"{(kidsClubValue ? "Yes" : "No"),-15}" +
                        $"{(restaurantValue ? "Yes" : "No"),-15}"
                    );
                }

// Hantera fall där ingen boende är tillgänglig
                if (!anyAvailable)
                {
                    Console.WriteLine("Inga boende tillgänglig.");
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
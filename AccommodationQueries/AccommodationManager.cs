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
                SELECT a.id, a.name, a.baseprice, a.rating, a.pool, a.eveningentertainment, a.kidsclub, a.restaurant
                FROM accommodation a
                WHERE
                    -- Search criteria filters
                    (@LocationId IS NULL OR a.location = @LocationId) AND
                    (@MinPrice IS NULL OR a.baseprice >= @MinPrice) AND
                    (@MaxPrice IS NULL OR a.baseprice <= @MaxPrice) AND
                    (@Pool IS NULL OR a.pool = @Pool) AND
                    (@EveningEntertainment IS NULL OR a.eveningentertainment = @EveningEntertainment) AND
                    (@KidsClub IS NULL OR a.kidsclub = @KidsClub) AND
                    (@Restaurant IS NULL OR a.restaurant = @Restaurant) AND

                    -- Exclude accommodations with overlapping bookings
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
                cmd.Parameters.Add("@Pool", NpgsqlDbType.Boolean).Value = pool.HasValue ? (object)pool.Value : DBNull.Value;
                cmd.Parameters.Add("@EveningEntertainment", NpgsqlDbType.Boolean).Value = eveningEntertainment.HasValue ? (object)eveningEntertainment.Value : DBNull.Value;
                cmd.Parameters.Add("@KidsClub", NpgsqlDbType.Boolean).Value = kidsClub.HasValue ? (object)kidsClub.Value : DBNull.Value;
                cmd.Parameters.Add("@Restaurant", NpgsqlDbType.Boolean).Value = restaurant.HasValue ? (object)restaurant.Value : DBNull.Value;

                await using var reader = await cmd.ExecuteReaderAsync();

                Console.WriteLine("\nAvailable Accommodations:");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------");
                Console.WriteLine($"{"ID",-5} {"Name",-45} {"Price",10} {"Rating",7} {"Pool",-5} {"Evening Ent.",-15} {"Kids Club",-10} {"Restaurant",-10}");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------");

                bool anyAvailable = false;

                while (await reader.ReadAsync())
                {
                    anyAvailable = true;
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    decimal baseprice = reader.GetDecimal(2);
                    double rating = reader.IsDBNull(3) ? 0.0 : reader.GetDouble(3);
                    bool poolValue = reader.GetBoolean(4);
                    bool eveningEntertainmentValue = reader.GetBoolean(5);
                    bool kidsClubValue = reader.GetBoolean(6);
                    bool restaurantValue = reader.GetBoolean(7);

                    Console.WriteLine(
                        $"{id,-5} " +
                        $"{name,-45} " +
                        $"{baseprice,10} USD " +
                        $"{rating,7:N1} " +
                        $"{(poolValue ? "Yes" : "No"),-5} " +
                        $"{(eveningEntertainmentValue ? "Yes" : "No"),-15} " +
                        $"{(kidsClubValue ? "Yes" : "No"),-10} " +
                        $"{(restaurantValue ? "Yes" : "No"),-10}"
                    );
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
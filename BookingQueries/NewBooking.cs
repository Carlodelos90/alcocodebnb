namespace alcocodebnb.BookingQueries;
using Npgsql;

public class NewBooking
    {
        private NpgsqlDataSource _database;

        public NewBooking(NpgsqlDataSource database)
        {
            _database = database;
        }
        
        
        public async void AddItem(int accommodationid, DateOnly startdate, DateOnly enddate, double totalprice, int numberofguests)
        {
            await using 
                (var cmd = _database.CreateCommand("INSERT INTO ITEMS " +
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
    }


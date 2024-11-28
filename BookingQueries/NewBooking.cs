namespace alcocodebnb.BookingQueries;

public class NewBooking
    {
        private readonly DatabaseConnection _dbConnection;

        public NewBooking(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        
        
        public async void AddItem(int accommodationid, DateOnly startdate, DateOnly enddate, double totalprice, int numberofguests)
        {
            await using 
                (var cmd = _dbConnection.CreateCommand("INSERT INTO ITEMS " +
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


using Npgsql;

namespace alcocodebnb;

    public class DatabaseQueries
    {
        private static NpgsqlDataSource? _database;

        public DatabaseQueries(NpgsqlDataSource database)
        {
            _database = database;
        }

        public static async void GetAllCustomers()
        {
            await using var cmd = _database?.CreateCommand("SELECT firstname, lastname FROM customer;");
            await using var reader = await cmd?.ExecuteReaderAsync()!; //The  "!" suppresses the nullable warning

            Console.WriteLine("Customers List:");
            while (reader.Read())
            {
                string firstName = reader.GetString(0);
                string lastName = reader.GetString(1);
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"- {firstName} {lastName}");
                Console.ResetColor();
            }
        }
    
        public static async void GetAllCustomersEmail()
        {
            await using var cmd = _database?.CreateCommand("SELECT id, firstname, lastname, email, phonenumber  FROM customer;");
            await using var reader = await cmd?.ExecuteReaderAsync()!;

            Console.WriteLine("Customers List:");
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string firstName = reader.GetString(1);
                string lastName = reader.GetString(2);
                string email = reader.GetString(3);
                string phonenumber = reader.GetString(4);
                
                Console.WriteLine($"- ID: {id} - {firstName,10} {lastName}'s is: {email,40} an the PHONE: {phonenumber,40}");
            }
        }
    }

    /* public void GetAllBookings()
     {
         string query = "SELECT id, customerid, accommodationid, startdate, enddate, totalprice, numberofguests, status FROM booking;";

         try
         {
             using var connection = _dbConnection.GetConnection();
             connection.Open();

             using var command = new NpgsqlCommand(query, connection);
             using var reader = command.ExecuteReader();

             Console.WriteLine("Bookings List:");
             while (reader.Read())
             {
                 int id = reader.GetInt32(0);
                 int customerId = reader.GetInt32(1);
                 int accommodationId = reader.GetInt32(2);
                 DateTime startDate = reader.GetDateTime(3);
                 DateTime endDate = reader.GetDateTime(4);
                 decimal totalPrice = reader.GetDecimal(5);
                 int numberOfGuests = reader.GetInt32(6);
                 string status = reader.GetString(7);

                 Console.WriteLine($"Booking ID: {id}, Customer ID: {customerId}, Accommodation ID: {accommodationId}, Start Date: {startDate:yyyy-MM-dd}, End Date: {endDate:yyyy-MM-dd}, Total Price: {totalPrice:C}, Guests: {numberOfGuests}, Status: {status}");
             }
         }
         catch (Exception ex)
         {
             Console.WriteLine($"Error fetching bookings: {ex.Message}");
         }
     }
 */

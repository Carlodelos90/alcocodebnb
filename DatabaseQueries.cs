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
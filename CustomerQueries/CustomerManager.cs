using Npgsql;

namespace alcocodebnb.CustomerQueries;

public class CustomerManager
{
    private readonly NpgsqlDataSource _database;

    public CustomerManager(NpgsqlDataSource database)
    {
        _database = database;
    }

    #region Add Customer

    public async Task AddCustomerAsync()
    {
        while (true)
        {
            Console.Write("Enter First Name: ");
            string? firstName = Console.ReadLine();

            Console.Write("Enter Last Name: ");
            string? lastName = Console.ReadLine();

            Console.Write("Enter Email: ");
            string? email = Console.ReadLine();

            Console.Write("Enter Phone Number: ");
            string? phoneNumber = Console.ReadLine();

            Console.Write("Enter Date of Birth (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth))
            {
                Console.WriteLine("Invalid Date of Birth.");
                continue;
            }
            
            await AddCustomerToDatabaseAsync(firstName, lastName, email, phoneNumber, dateOfBirth);

            Console.WriteLine("\nCustomer added successfully.");

            Console.Write("\nDo you want to add another customer? (y/n): ");
            string? choice = Console.ReadLine();
            if (choice?.ToLower() != "y")
            {
                break;
            }
        }
    }

    private async Task AddCustomerToDatabaseAsync(string firstName, string lastName, string email, string phoneNumber, DateTime dateOfBirth)
    {
        string query = @"
            INSERT INTO customer (firstname, lastname, email, phonenumber, dateofbirth)
            VALUES ($1, $2, $3, $4, $5)
            RETURNING id;";

        try
        {
            await using var cmd = _database.CreateCommand(query);
            cmd.Parameters.AddWithValue(firstName);
            cmd.Parameters.AddWithValue(lastName);
            cmd.Parameters.AddWithValue(email);
            cmd.Parameters.AddWithValue(phoneNumber);
            cmd.Parameters.AddWithValue(dateOfBirth);

            // Execute the command and get the new customer ID
            int newCustomerId = (int)(await cmd.ExecuteScalarAsync())!;

            Console.WriteLine("\nCustomer ID: " + newCustomerId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding new customer: {ex.Message}");
        }
    }

    #endregion

    #region Add Guest

    public async Task AddGuestAsync()
    {
        while (true)
        {
            Console.Write("Enter guest's First Name: ");
            string? firstName = Console.ReadLine();

            Console.Write("Enter guest's Last Name: ");
            string? lastName = Console.ReadLine();

            Console.Write("Enter guest's Email: ");
            string? email = Console.ReadLine();

            Console.Write("Enter guest's Phone Number: ");
            string? phoneNumber = Console.ReadLine();

            Console.Write("Enter guest's Date of Birth (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth))
            {
                Console.WriteLine("Invalid Date of Birth.");
                continue;
            }
            
            Console.Write("Enter guest's booking's ID: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid Booking ID.");
                continue;
            }
            
            bool isSuccess = await AddGuestToDatabaseAsync(firstName, lastName, email, phoneNumber, dateOfBirth, bookingId);

            if (isSuccess)
            {
                Console.WriteLine("\nGuest added successfully.");
                bool isUpdated = await IncrementNumberOfGuestsAsync(bookingId);
                if (isUpdated)
                {
                    Console.WriteLine("Number of guests incremented by 1.");
                }
                else
                {
                    Console.WriteLine("Failed to update number of guests.");
                }
            }

            Console.Write("\nDo you want to add another guest? (y/n): ");
            string? choice = Console.ReadLine();
            if (choice?.ToLower() != "y")
            {
                break;
            }
        }
    }

    private async Task<bool> AddGuestToDatabaseAsync(string firstName, string lastName, string email, string phoneNumber, DateTime dateOfBirth, int bookingId)
    {
        string query = @"
            INSERT INTO guest (firstname, lastname, email, phonenumber, dateofbirth, bookingid)
            VALUES ($1, $2, $3, $4, $5, $6)
            RETURNING id;";

        try
        {
            await using var cmd = _database.CreateCommand(query);
            cmd.Parameters.AddWithValue(firstName);
            cmd.Parameters.AddWithValue(lastName);
            cmd.Parameters.AddWithValue(email);
            cmd.Parameters.AddWithValue(phoneNumber);
            cmd.Parameters.AddWithValue(dateOfBirth);
            cmd.Parameters.AddWithValue(bookingId);

            // Execute the command and get the new guest ID
            int newGuestId = (int)(await cmd.ExecuteScalarAsync())!;

            Console.WriteLine("\nGuest ID: " + newGuestId);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding new guest: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> IncrementNumberOfGuestsAsync(int bookingId)
    {
        string query = @"
            UPDATE booking
            SET numberofguests = numberofguests + 1
            WHERE id = $1;";

        try
        {
            await using var cmd = _database.CreateCommand(query);
            cmd.Parameters.AddWithValue(bookingId);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating number of guests: {ex.Message}");
            return false;
        }
    }

    #endregion
}
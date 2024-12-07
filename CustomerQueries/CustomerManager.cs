using Npgsql;

namespace alcocodebnb.CustomerQueries;

public class CustomerManager(NpgsqlDataSource database)
{
    #region Add Customer

    public async Task AddCustomerAsync()
    {
        while (true)
        {
            string firstName = PromptForNonEmptyString("Enter First Name: ");
            string lastName = PromptForNonEmptyString("Enter Last Name: ");
            string email = PromptForNonEmptyString("Enter Email: ");
            string phoneNumber = PromptForNonEmptyString("Enter Phone Number: ");
            
            DateTime dateOfBirth = PromptForValidDate("Enter Date of Birth (yyyy-mm-dd): ");

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
            await using var cmd = database.CreateCommand(query);
            cmd.Parameters.AddWithValue(firstName);
            cmd.Parameters.AddWithValue(lastName);
            cmd.Parameters.AddWithValue(email);
            cmd.Parameters.AddWithValue(phoneNumber);
            cmd.Parameters.AddWithValue(dateOfBirth);

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
            string firstName = PromptForNonEmptyString("Enter guest's First Name: ");
            string lastName = PromptForNonEmptyString("Enter guest's Last Name: ");
            string email = PromptForNonEmptyString("Enter guest's Email: ");
            string phoneNumber = PromptForNonEmptyString("Enter guest's Phone Number: ");
            DateTime dateOfBirth = PromptForValidDate("Enter guest's Date of Birth (yyyy-mm-dd): ");

            int bookingId = PromptForValidInt("Enter guest's booking's ID: ");

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
            await using var cmd = database.CreateCommand(query);
            cmd.Parameters.AddWithValue(firstName);
            cmd.Parameters.AddWithValue(lastName);
            cmd.Parameters.AddWithValue(email);
            cmd.Parameters.AddWithValue(phoneNumber);
            cmd.Parameters.AddWithValue(dateOfBirth);
            cmd.Parameters.AddWithValue(bookingId);

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
            await using var cmd = database.CreateCommand(query);
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

    #region Helper Methods
    
    private string PromptForNonEmptyString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                return input;
            }

            Console.WriteLine("Input cannot be empty. Please try again.\n");
        }
    }
    
    private DateTime PromptForValidDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (DateTime.TryParse(input, out DateTime date))
            {
                return date;
            }

            Console.WriteLine("Invalid date format. Please try again.\n");
        }
    }
    private int PromptForValidInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value))
            {
                return value;
            }

            Console.WriteLine("Invalid number. Please try again.\n");
        }
    }

    #endregion
}
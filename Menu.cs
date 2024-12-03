using alcocodebnb.BookingQueries;
using alcocodebnb.CustomerQueries;
using alcocodebnb.AccommodationQueries;

namespace alcocodebnb;

public class Menu
{
    static readonly DatabaseConnection Db = new();
    CustomerManager _customerManager = new CustomerManager(Db.Connection());
    AccommodationManager _accommodationManager = new(Db.Connection());

    public Menu()
    {
        DatabaseQueries queries = new(Db.Connection());
        CancelBooking cancel = new(Db.Connection());
        NewBooking addBooking = new(Db.Connection());
        CustomerManager customer = new CustomerManager(Db.Connection());
        AccommodationManager accommodationManager = new(Db.Connection());
        EditBooking editBooking = new(Db.Connection());
    }

    #region Start method of the menu
    public async Task Start()
    {
        while (true)
        {
            DisplayMainMenu();
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await ManageBookingsAsync();
                    break;
                case "2":
                    await ManageCustomersAsync();
                    break;
                case "3":
                    ManageAccommodationsAsync();
                    break;
                case "4":
                    Console.WriteLine("Goodbye!");
                    return;
                case "5":
                    await ManageMiscellaneousAsync();
                    break;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
#endregion
    
    #region Menu Display Methods

    private void DisplayMainMenu()
    {
        Console.Clear();
        Console.WriteLine("------- Welcome to AlcoCodeBNB --------");
        Console.WriteLine("\n1. Manage Bookings" +
                          "\n2. Manage Customers" +
                          "\n3. Manage Accommodations" +
                          "\n4. Exit" +
                          "\n5. Miscellaneous");
        Console.Write("\nChoose an option: ");
    }

    private void DisplaySubMenu(string title, string[] options)
    {
        Console.Clear();
        Console.WriteLine($"------- {title} --------");
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"\n{i + 1}. {options[i]}");
        }
        Console.Write("\nChoose an option: ");
    }

    #endregion

    #region Manage Bookings

    private async Task ManageBookingsAsync()
    {
        while (true)
        {
            string[] bookingOptions = new[]
            {
                "New Booking (OBS! if new customer, make a new customer account first)",
                "Cancel Booking",
                "Change Booking",
                "Filter Price",
                "Filter Review",
                "Add Guests to the Booking",
                "Sort by distance to beach",
                "Sort by distance to center",
                "Back to Main Menu"
            };

            DisplaySubMenu("Manage Bookings", bookingOptions);
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    await CreateNewBookingAsync();
                    break;
                case "2":
                    CancelBookingAsync();
                    break;
                case "3":
                    await ChangeBookingAsync();
                    break;
                case "4":
                    FilterPriceAsync();
                    break;
                case "5":
                    FilterReviewAsync();
                    break;
                case "6":
                    await AddGuestsToBookingAsync();
                    break;
                case "7":
                    //await SortByDistanceToBeach();
                    Console.WriteLine("Sort by beach");
                    Console.ReadLine();
                    break;
                case "8":
                    //await SortByDistanceToCenter();
                    Console.WriteLine("Sort by Center");
                    Console.ReadLine();
                    break;
                
                case "9":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task CreateNewBookingAsync()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("------- New Booking --------");

            // Display all locations
            await NewBooking.AllLocationsAsync();

            Console.Write("\nEnter Location ID: ");
            if (!int.TryParse(Console.ReadLine(), out int locationId))
            {
                Console.WriteLine("Invalid Location ID.");
                return;
            }

            // Show accommodations based on location
            await NewBooking.ShowAccommodationsAsync(locationId);

            Console.Write("\nEnter Accommodation ID: ");
            if (!int.TryParse(Console.ReadLine(), out int accommodationId))
            {
                Console.WriteLine("Invalid Accommodation ID.");
                return;
            }

            Console.Write("Enter Customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Invalid Customer ID.");
                return;
            }

            Console.Write("Enter Start Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
            {
                Console.WriteLine("Invalid Start Date.");
                return;
            }

            Console.Write("Enter End Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
            {
                Console.WriteLine("Invalid End Date.");
                return;
            }

            Console.Write("Enter Number of Guests: ");
            if (!int.TryParse(Console.ReadLine(), out int numberOfGuests))
            {
                Console.WriteLine("Invalid number of guests.");
                return;
            }

            // Add the new booking
            await NewBooking.AddNewBookingAsync(customerId, accommodationId, startDate, endDate, numberOfGuests);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating booking: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }
    }

    private void CancelBookingAsync()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("------- Cancel Booking --------");
            EditBooking.GetAllBookingsAsync();

            Console.Write("Enter the ID of the booking you want to cancel: ");
            if (int.TryParse(Console.ReadLine(), out int cancelId))
            {
                CancelBooking.DeleteBooking(cancelId);
                Console.WriteLine("Booking cancelled successfully.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cancelling booking: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private async Task ChangeBookingAsync()
    {
        while (true)
        {
            string[] editBookingOptions = new[]
            {
                "Change the Date of Your Booking",
                "Change the Number of Guests",
                "Change Extra Addons",
                "Exit"
            };

            DisplaySubMenu("Edit Bookings", editBookingOptions);
            string? inputEdit = Console.ReadLine();

            switch (inputEdit)
            {
                case "1":
                    await ChangeBookingDateAsync();
                    break;
                case "2":
                    await ChangeNumberOfGuestsAsync();
                    break;
                case "3":
                    ChangeExtraAddonsAsync();
                    break;
                case "4":
                    Console.WriteLine("Exiting... Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task ChangeBookingDateAsync()
    {
        try
        {
            Console.Clear();
            EditBooking.GetAllBookingsAsync();

            Console.Write("Enter the Booking ID: ");
            if (!int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.WriteLine("Invalid Booking ID.");
                return;
            }

            Console.Write("Enter the New Start Date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime newStartDate))
            {
                Console.WriteLine("Invalid Start Date.");
                return;
            }

            Console.Write("Enter the New End Date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime newEndDate))
            {
                Console.WriteLine("Invalid End Date.");
                return;
            }

            if (newEndDate <= newStartDate)
            {
                Console.WriteLine("End date must be after start date.");
                return;
            }

            await EditBooking.ChangeBookingDateAsync(bookingId, newStartDate, newEndDate);
            Console.WriteLine("Booking date updated successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error changing booking date: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private async Task ChangeNumberOfGuestsAsync()
    {
        try
        {
            Console.Clear();
            EditBooking.GetAllBookingsAsync();
            Console.WriteLine("Please enter guest's details:");
            await _customerManager.AddGuestAsync();
            Console.WriteLine("Number of guests updated successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error changing number of guests: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private void ChangeExtraAddonsAsync()
    {
        try
        {
            Console.Clear();
            EditBooking.GetAllAddonsAsync();

            Console.Write("Enter the Booking ID: ");
            if (!int.TryParse(Console.ReadLine(), out int extraBookingId))
            {
                Console.WriteLine("Invalid Booking ID.");
                return;
            }

            Console.Write("Enter the Extra Service ID: ");
            if (!int.TryParse(Console.ReadLine(), out int extraServiceId))
            {
                Console.WriteLine("Invalid Extra Service ID.");
                return;
            }

            Console.Write("Enter the Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("Invalid Quantity.");
                return;
            }

            //EditBooking.ChangeBoardOption(extraServiceId, quantity, extraBookingId);
            Console.WriteLine("Extra addons updated successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error changing extra addons: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private void FilterPriceAsync()
    {
        while (true)
        {
            string[] filterPriceOptions = new[]
            {
                "Sort accommodations by price ascending",
                "Sort accommodations by price descending",
                "Back to Manage Bookings"
            };

            DisplaySubMenu("Sort Accommodations by Price", filterPriceOptions);
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    NewBooking.FilterPriceAscAsync();
                    Console.WriteLine("Press any key to go back to 'Manage Bookings'");
                    Console.ReadKey();
                    break;
                case "2":
                    NewBooking.FilterPriceDescAsync();
                    Console.WriteLine("Press any key to go back to 'Manage Bookings'");
                    Console.ReadKey();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void FilterReviewAsync()
    {
        try
        {
            Console.Clear();
            NewBooking.FilterReviewAsync();
            Console.WriteLine("Filter Review functionality coming soon...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error filtering reviews: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private async Task AddGuestsToBookingAsync()
    {
        try
        {
            Console.WriteLine("Please enter guest's details:");
            await _customerManager.AddGuestAsync();
            Console.WriteLine("Guests added successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding guests: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }
    }

    #endregion

    #region Manage Customers

    private async Task ManageCustomersAsync()
    {
        while (true)
        {
            string[] customerOptions = new[]
            {
                "View All Customers",
                "View Customer's Details",
                "Add Customer",
                "Back to Main Menu"
            };

            DisplaySubMenu("Manage Customers", customerOptions);
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ViewAllCustomersAsync();
                    break;
                case "2":
                    ViewCustomerDetailsAsync();
                    break;
                case "3":
                    await AddCustomerAsync();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void ViewAllCustomersAsync()
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.Red;
            DatabaseQueries.GetAllCustomers();
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error viewing customers: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }

    private void ViewCustomerDetailsAsync()
    {
        try
        {
            Console.WriteLine("\nEnter customer details (e.g., Name, Email, Phone):");
            DatabaseQueries.GetAllCustomersEmail();
            Console.WriteLine("View Customer Details functionality coming soon...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error viewing customer details: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }
    }

    private async Task AddCustomerAsync()
    {
        try
        {
            Console.WriteLine("\nEnter customer details (e.g., Name, Email, Phone):");
            await _customerManager.AddCustomerAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding customer: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }

    #endregion

    #region Manage Accommodations

    private void ManageAccommodationsAsync()
    {
        while (true)
        {
            string[] accommodationOptions = new[]
            {
                "Search Available Accommodations",
                "Back to Main Menu"
            };

            DisplaySubMenu("Manage Accommodations", accommodationOptions);
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    SearchAvailableAccommodationsAsync();
                    break;
                case "2":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void SearchAvailableAccommodationsAsync()
    {
        try
        {
            Console.Write("Enter Start Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
            {
                Console.WriteLine("Invalid Start Date.");
                return;
            }

            Console.Write("Enter End Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
            {
                Console.WriteLine("Invalid End Date.");
                return;
            }

            if (endDate <= startDate)
            {
                Console.WriteLine("End date must be after start date.");
                return;
            }

            Console.Write("Enter Location ID (or leave blank): ");
            string? locationInput = Console.ReadLine();
            int? locationId = null;
            if (!string.IsNullOrWhiteSpace(locationInput) && int.TryParse(locationInput, out int locId))
            {
                locationId = locId;
            }

            Console.Write("Enter Minimum Price (or leave blank): ");
            string? minPriceInput = Console.ReadLine();
            decimal? minPrice = null;
            if (!string.IsNullOrWhiteSpace(minPriceInput) && decimal.TryParse(minPriceInput, out decimal minPriceValue))
            {
                minPrice = minPriceValue;
            }

            Console.Write("Enter Maximum Price (or leave blank): ");
            string? maxPriceInput = Console.ReadLine();
            decimal? maxPrice = null;
            if (!string.IsNullOrWhiteSpace(maxPriceInput) && decimal.TryParse(maxPriceInput, out decimal maxPriceValue))
            {
                maxPrice = maxPriceValue;
            }

            // Search for available accommodations
            SearchAvailableAccommodationsAsync(startDate, endDate, locationId, minPrice, maxPrice);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching accommodations: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }

    private void SearchAvailableAccommodationsAsync(DateTime startDate, DateTime endDate, int? locationId, decimal? minPrice, decimal? maxPrice)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Manage Miscellaneous

    private async Task ManageMiscellaneousAsync()
    {
        while (true)
        {
            string[] miscOptions = new[]
            {
                "Run Synchronous Operation",
                "Run Asynchronous Operation",
                "Calc Test",
                "Back to Main Menu"
            };

            DisplaySubMenu("Miscellaneous Operations", miscOptions);
            string? inputMisc = Console.ReadLine();

            switch (inputMisc)
            {
                case "1":
                    await RunSynchronousOperationAsync();
                    break;
                case "2":
                    await RunAsynchronousOperationAsync();
                    break;
                case "3":
                    // Implement Calc test functionality when ready
                    Console.WriteLine("Calc Test functionality coming soon...");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task RunSynchronousOperationAsync()
    {
        try
        {
            Console.WriteLine("Starting synchronous operation...");
            LongRunningOperation();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Main thread working... {i + 1}");
                await Task.Delay(1000); // Simulate work asynchronously
            }

            Console.WriteLine("Finished synchronous operation.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during synchronous operation: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }
    }

    private async Task RunAsynchronousOperationAsync()
    {
        try
        {
            Console.WriteLine("Starting asynchronous operation...");
            Task longRunningTask = LongRunningOperationAsync();

            Console.WriteLine("Doing other work while waiting...");

            // Simulate other asynchronous work
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Main thread working... {i + 1}");
                await Task.Delay(1000); // Simulate work asynchronously
            }

            await longRunningTask;
            Console.WriteLine("Finished asynchronous operation.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during asynchronous operation: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }
    }

    #endregion

    #region Long-Running Operations

    private void LongRunningOperation()
    {
        Thread.Sleep(5000); // Simulates a long operation
        Console.WriteLine("Long-running operation completed.");
    }

    private async Task LongRunningOperationAsync()
    {
        await Task.Delay(5000); // Simulates a long operation asynchronously
        Console.WriteLine("Long-running operation completed.");
    }

    #endregion
}
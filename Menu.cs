using alcocodebnb.BookingQueries;
using alcocodebnb.CustomerQueries;
using alcocodebnb.AccommodationQueries;
using alcocodebnb.ConsoleUtilities;
using System;
using System.Threading.Tasks;

namespace alcocodebnb
{
    public class Menu
    {
        static readonly DatabaseConnection Db = new();
        CustomerManager _customerManager = new CustomerManager(Db.Connection());
        AccommodationManager _accommodationManager = new(Db.Connection());
        private readonly DatabaseQueries _queries;
        private readonly ApplicationConsole _console = new ApplicationConsole();
        static DatabaseConnection db = new();

        public Menu()
        {
            DatabaseQueries queries = new(Db.Connection());
            CancelBooking cancel = new(Db.Connection());
            NewBooking addBooking = new(Db.Connection());
            CustomerManager customer = new CustomerManager(Db.Connection());
            AccommodationManager accommodationManager = new(Db.Connection());
            EditBooking editBooking = new(Db.Connection());
            _queries = new DatabaseQueries(Db.Connection());
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
                        _console.WriteSuccess("Goodbye!");
                        return;
                    case "5":
                        await ManageMiscellaneousAsync();
                        break;
                    default:
                        _console.WriteError("Invalid option. Press any key to try again.");
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
            _console.WriteMenuTitle("------- Welcome to AlcoCodeBNB --------");
            _console.WriteInfo("\n1. Manage Bookings" +
                              "\n2. Manage Customers" +
                              "\n3. Manage Accommodations" +
                              "\n4. Exit" +
                              "\n5. Miscellaneous");
            _console.WriteInfo("Choose an option: ");
        }

        private void DisplaySubMenu(string title, string[] options)
        {
            Console.Clear();
            _console.WriteMenuTitle($"------- {title} --------");
            for (int i = 0; i < options.Length; i++)
            {
                _console.WriteMenuOption($"{i + 1}. {options[i]}");
            }
            _console.WriteInfo("Choose an option: ");
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
                        await CancelBookingAsync();
                        break;
                    case "3":
                        await ChangeBookingAsync();
                        break;
                    case "4":
                        await FilterPriceAsync();
                        break;
                    case "5":
                        await FilterReviewAsync();
                        break;
                    case "6":
                        await AddGuestsToBookingAsync();
                        break;
                    case "7":
                        await SortByDistanceToBeach();
                        Console.ReadLine();
                        break;
                    case "8":
                        await NewBooking.SortByDistanceToCenterAsync();
                        Console.ReadLine();
                        break;

                    case "9":
                        return;
                    default:
                        _console.WriteError("Invalid option. Press any key to try again.");
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
                _console.WriteMenuTitle("------- New Booking --------");
                
                await NewBooking.AllLocationsAsync();

                _console.WriteInfo("Enter Location ID: ");
                if (!int.TryParse(Console.ReadLine(), out int locationId))
                {
                    _console.WriteError("Invalid Location ID.");
                    return;
                }
                
                await NewBooking.ShowAccommodationsAsync(locationId);

                _console.WriteInfo("Enter Accommodation ID: ");
                if (!int.TryParse(Console.ReadLine(), out int accommodationId))
                {
                    _console.WriteError("Invalid Accommodation ID.");
                    return;
                }

                _console.WriteInfo("Enter Customer ID: ");
                if (!int.TryParse(Console.ReadLine(), out int customerId))
                {
                    _console.WriteError("Invalid Customer ID.");
                    return;
                }

                _console.WriteInfo("Enter Start Date (yyyy-mm-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                {
                    _console.WriteError("Invalid Start Date.");
                    return;
                }

                _console.WriteInfo("Enter End Date (yyyy-mm-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                {
                    _console.WriteError("Invalid End Date.");
                    return;
                }

                _console.WriteInfo("Enter Number of Guests: ");
                if (!int.TryParse(Console.ReadLine(), out int numberOfGuests))
                {
                    _console.WriteError("Invalid number of guests.");
                    return;
                }
                
                await NewBooking.AddNewBookingAsync(customerId, accommodationId, startDate, endDate, numberOfGuests);
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error creating booking: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("Press any key to return...");
                Console.ReadKey();
            }
        }

        private async Task CancelBookingAsync()
        {
            try
            {
                Console.Clear();
                _console.WriteMenuTitle("------- Cancel Booking --------");
                EditBooking.GetAllBookingsAsync();

                _console.WriteInfo("Enter the ID of the booking you want to cancel: ");
                if (int.TryParse(Console.ReadLine(), out int cancelId))
                {
                    CancelBooking.DeleteBooking(cancelId);
                    _console.WriteSuccess("Booking cancelled successfully.");
                }
                else
                {
                    _console.WriteError("Invalid input. Please enter a valid number.");
                }
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error cancelling booking: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("Press any key to continue...");
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
                    "Change Addons",
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
                        await UpdateBookingExtrasAsync();
                        break;

                    case "4":
                        _console.WriteSuccess("Exiting... Goodbye!");
                        return;
                    default:
                        _console.WriteError("Invalid option. Press any key to try again.");
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

                _console.WriteInfo("Enter the Booking ID: ");
                if (!int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    _console.WriteError("Invalid Booking ID.");
                    return;
                }

                _console.WriteInfo("Enter the New Start Date (yyyy-MM-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime newStartDate))
                {
                    _console.WriteError("Invalid Start Date.");
                    return;
                }

                _console.WriteInfo("Enter the New End Date (yyyy-MM-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime newEndDate))
                {
                    _console.WriteError("Invalid End Date.");
                    return;
                }

                if (newEndDate <= newStartDate)
                {
                    _console.WriteError("End date must be after start date.");
                    return;
                }

                await EditBooking.ChangeBookingDateAsync(bookingId, newStartDate, newEndDate);
                _console.WriteSuccess("Booking date updated successfully!");
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error changing booking date: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private async Task ChangeNumberOfGuestsAsync()
        {
            try
            {
                Console.Clear();
                await _queries.GetAllCustomers();
                _console.WriteInfo("Please enter guest's details:");
                await _customerManager.AddGuestAsync();
                _console.WriteSuccess("Number of guests updated successfully!");
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error changing number of guests: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }
        
        public async Task UpdateBookingExtrasAsync()
        {
            try
            {
                _console.WriteInfo("Enter the Booking ID: ");
                if (!int.TryParse(Console.ReadLine(), out int bookingId))
                {
                    _console.WriteError("Invalid Booking ID.");
                    return;
                }

                _console.WriteInfo("Enter the Extra Service ID: ");
                if (!int.TryParse(Console.ReadLine(), out int extraServiceId))
                {
                    _console.WriteError("Invalid Extra Service ID.");
                    return;
                }

                _console.WriteInfo("Enter the Quantity: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity))
                {
                    _console.WriteError("Invalid Quantity.");
                    return;
                }
                await EditBooking.ChangeBookingOptionsAsync(bookingId, extraServiceId, quantity);
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error changing extra addons: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private async Task FilterPriceAsync()
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
                        await NewBooking.FilterPriceAscAsync();
                        _console.WriteInfo("Press any key to go back to 'Manage Bookings'");
                        Console.ReadKey();
                        break;
                    case "2":
                        await NewBooking.FilterPriceDescAsync();
                        _console.WriteInfo("Press any key to go back to 'Manage Bookings'");
                        Console.ReadKey();
                        break;
                    case "3":
                        return;
                    default:
                        _console.WriteError("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task FilterReviewAsync()
        {
            try
            {
                Console.Clear();
                await NewBooking.FilterReviewAsync();
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error filtering reviews: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private async Task SortByDistanceToBeach()
        {
            try
            {
                Console.Clear();
                await NewBooking.SortByDistanceToBeach();
                _console.WriteInfo("Sort by beach completed.");
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error sorting by distance to beach: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private async Task AddGuestsToBookingAsync()
        {
            try
            {
                _console.WriteInfo("Please enter guest's details:");
                await _customerManager.AddGuestAsync();
                _console.WriteSuccess("Guests added successfully!");
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error adding guests: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("Press any key to return...");
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
                        await ViewAllCustomersAsync();
                        break;
                    case "2":
                        await ViewCustomerDetailsAsync();
                        break;
                    case "3":
                        await AddCustomerAsync();
                        break;
                    case "4":
                        return;
                    default:
                        _console.WriteError("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task ViewAllCustomersAsync()
        {
            try
            {
                Console.Clear();
                _console.WriteMenuTitle("------- All Customers --------");
                await _queries.GetAllCustomers();
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error viewing customers: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("\nPress any key to return...");
                Console.ReadKey();
            }
        }

        private async Task ViewCustomerDetailsAsync()
        {
            try
            {
                _console.WriteInfo("Enter customer details (e.g., Name, Email, Phone):");
                await _queries.GetAllCustomersEmail();
                _console.WriteInfo("View Customer Details functionality coming soon...");
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error viewing customer details: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("Press any key to return...");
                Console.ReadKey();
            }
        }

        private async Task AddCustomerAsync()
        {
            try
            {
                _console.WriteInfo("Enter customer details (e.g., Name, Email, Phone):");
                await _customerManager.AddCustomerAsync();
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error adding customer: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("\nPress any key to return...");
                Console.ReadKey();
            }
        }

        #endregion

        #region Manage Accommodations

        private async Task ManageAccommodationsAsync()
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
                        await HandleSearchAvailableAccommodationsAsync();
                        break;
                    case "2":
                        return;
                    default:
                        _console.WriteError("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task HandleSearchAvailableAccommodationsAsync()
        {
            try
            {
                _console.WriteInfo("Enter Start Date (yyyy-mm-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                {
                    _console.WriteError("Invalid Start Date.");
                    return;
                }

                _console.WriteInfo("Enter End Date (yyyy-mm-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                {
                    _console.WriteError("Invalid End Date.");
                    return;
                }

                if (endDate <= startDate)
                {
                    _console.WriteError("End date must be after start date.");
                    return;
                }

                _console.WriteInfo("Enter Location ID (or leave blank): ");
                string? locationInput = Console.ReadLine();
                int? locationId = null;
                if (!string.IsNullOrWhiteSpace(locationInput) && int.TryParse(locationInput, out int locId))
                {
                    locationId = locId;
                }

                _console.WriteInfo("Enter Minimum Price (or leave blank): ");
                string? minPriceInput = Console.ReadLine();
                decimal? minPrice = null;
                if (!string.IsNullOrWhiteSpace(minPriceInput) && decimal.TryParse(minPriceInput, out decimal minPriceValue))
                {
                    minPrice = minPriceValue;
                }

                _console.WriteInfo("Enter Maximum Price (or leave blank): ");
                string? maxPriceInput = Console.ReadLine();
                decimal? maxPrice = null;
                if (!string.IsNullOrWhiteSpace(maxPriceInput) && decimal.TryParse(maxPriceInput, out decimal maxPriceValue))
                {
                    maxPrice = maxPriceValue;
                }
                bool? pool = PromptYesNo("Do you require a Pool? (yes/no or leave blank for any): ");
                bool? eveningEntertainment = PromptYesNo("Do you require Evening Entertainment? (yes/no or leave blank for any): ");
                bool? kidsClub = PromptYesNo("Do you require a Kids Club? (yes/no or leave blank for any): ");
                bool? restaurant = PromptYesNo("Do you require a Restaurant? (yes/no or leave blank for any): ");

                await _accommodationManager.SearchAvailableAccommodationsAsync(
                    startDate,
                    endDate,
                    locationId,
                    minPrice,
                    maxPrice,
                    pool,
                    eveningEntertainment,
                    kidsClub,
                    restaurant
                );
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error searching accommodations: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("\nPress any key to return...");
                Console.ReadKey();
            }
        }
        
        private bool? PromptYesNo(string message)
        {
            _console.WriteInfo(message);
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (input == "yes" || input == "y")
                return true;
            else if (input == "no" || input == "n")
                return false;
            else
                return null;
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
                        _console.WriteColoredLine("Calc Test functionality coming soon...", ConsoleColor.Cyan);
                        _console.WriteInfo("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case "4":
                        return;
                    default:
                        _console.WriteError("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task RunSynchronousOperationAsync()
        {
            try
            {
                _console.WriteColoredLine("Starting synchronous operation...", ConsoleColor.Yellow);
                LongRunningOperation();

                for (int i = 0; i < 10; i++)
                {
                    _console.WriteInfo($"Main thread working... {i + 1}");
                    await Task.Delay(1000);
                }

                _console.WriteSuccess("Finished synchronous operation.");
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error during synchronous operation: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("Press any key to return...");
                Console.ReadKey();
            }
        }

        private async Task RunAsynchronousOperationAsync()
        {
            try
            {
                _console.WriteColoredLine("Starting asynchronous operation...", ConsoleColor.Yellow);
                Task longRunningTask = LongRunningOperationAsync();

                _console.WriteInfo("Doing other work while waiting...");
                
                for (int i = 0; i < 10; i++)
                {
                    _console.WriteInfo($"Main thread working... {i + 1}");
                    await Task.Delay(1000);
                }

                await longRunningTask;
                _console.WriteSuccess("Finished asynchronous operation.");
            }
            catch (Exception ex)
            {
                _console.WriteError($"Error during asynchronous operation: {ex.Message}");
            }
            finally
            {
                _console.WriteInfo("Press any key to return...");
                Console.ReadKey();
            }
        }

        #endregion

        #region Long-Running Operations

        private void LongRunningOperation()
        {
            Thread.Sleep(5000);
            _console.WriteColoredLine("Long-running operation completed.", ConsoleColor.Green);
        }

        private async Task LongRunningOperationAsync()
        {
            await Task.Delay(5000);
            _console.WriteColoredLine("Long-running operation completed.", ConsoleColor.Green);
        }

        #endregion
    }
}
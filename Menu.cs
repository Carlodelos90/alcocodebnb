using System.Diagnostics;
using alcocodebnb.BookingQueries;
using alcocodebnb.CustomerQueries;
using alcocodebnb.AccommodationQueries;

//using alcocodebnb.CustomerQueries;

namespace alcocodebnb;

public class Menu
{
    static DatabaseConnection db = new();
    CustomerManager _customerManager = new CustomerManager(db.Connection());
    public Menu()
    {
        DatabaseQueries queries = new(db.Connection());
        CancelBooking cancel = new(db.Connection());
        NewBooking addBooking = new(db.Connection());
        CustomerManager customer= new CustomerManager(db.Connection());
        AccommodationManager accommodationManager = new(db.Connection());
    }

    public async Task Start()
    {
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("------- Welcome to AlcoCodeBNB --------");
            Console.WriteLine("\n1. Manage Bookings" +
                              "\n2. Customers" +
                              "\n3. Accommodations" +
                              "\n4. Exit" +
                              "\n5. Misc.");
            Console.Write("\nChoose an option: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ManageBookings();
                    break;
                case "2":
                    await ManageCustomers();
                    break;
                case "3":
                    await ManageAccommodations();
                    break;
                case "4":
                    Console.WriteLine("Goodbye!");
                    return;
                case "5":
                    Console.WriteLine("\nChoose an option:" +
                                      "\n1. Run Synchronous Operation" +
                                      "\n2. Run Asynchronous Operation"+
                                      "\n3. Calc test");
                    Console.Write("\nEnter your choice: ");
                    string? inputMisc = Console.ReadLine();
                    switch(inputMisc)
                    {
                        case "1":
                            Console.WriteLine("Starting synchronous operation...");
                            LongRunningOperation();
                            
                            for (int i = 0; i < 10; i++)
                            {
                                Console.WriteLine($"Main thread working... {i+1}");
                                await Task.Delay(1000); // Simulate work
                            }
                            Console.WriteLine("Finished synchronous operation.");
                            break;
        
                        case "2":
                            Console.WriteLine("Starting  asynchronous (INTE synchronous) operation...");
                            var task = LongRunningOperationAsync();
                            Console.WriteLine("Doing other work while waiting...");
                            // Simulate other work
                            for (int i = 0; i < 10; i++)
                            {
                                Console.WriteLine($"Main thread working... {i+1}");
                                await Task.Delay(1000); // Simulate work
                            }
                            await task;
                            Console.WriteLine("Finished asynchronous operation.");
                            break;
                        
                        case "3":
                            //Console.WriteLine(calc);
                            break;

        
                        default:
                            Console.WriteLine("Invalid option. Press any key to try again.");
                            Console.ReadKey();
                            break;
                    }
                    break;

                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
    
    void LongRunningOperation()
    {
        Thread.Sleep(5000); // Simulates a long operation
        Console.WriteLine("Long-running operation completed.");
    }

    async Task LongRunningOperationAsync()
    {
        await Task.Delay(5000); // Simulates a long operation
        Console.WriteLine("Long-running operation completed.");
    }

    private async void ManageBookings()
    {
        while (true)
        {
            //Console.Clear();
            Console.WriteLine("------- Manage Bookings --------");
            Console.WriteLine("\n1. New Booking (OBS! if new customer, make a new customer account first)" +
                              "\n2. Cancel Booking" +
                              "\n3. Change Booking" +
                              "\n4. Filter price" +
                              "\n5. Filter review" +
                              "\n6. Add guest's to the booking" +
                              "\n7. Back to Main Menu");
            
            Console.WriteLine("\nChoose an option: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    // Collect booking details
                    NewBooking.AllLocations();

                    Console.Write("\nEnter Location ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int locationId))
                    {
                        Console.WriteLine("Invalid Location ID.");
                        break;
                    }

                    NewBooking.ShowAccommodations(locationId);

                    Console.Write("\nEnter Accommodation ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int accommodationId))
                    {
                        Console.WriteLine("Invalid Accommodation ID.");
                        break;
                    }

                    Console.Write("Enter Customer ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int customerId))
                    {
                        Console.WriteLine("Invalid Customer ID.");
                        break;
                    }

                    Console.Write("Enter Start Date (yyyy-mm-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                    {
                        Console.WriteLine("Invalid Start Date.");
                        break;
                    }

                    Console.Write("Enter End Date (yyyy-mm-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                    {
                        Console.WriteLine("Invalid End Date.");
                        break;
                    }

                    if (endDate <= startDate)
                    {
                        Console.WriteLine("End date must be after start date.");
                        break;
                    }

                    Console.Write("Enter Number of Guests: ");
                    if (!int.TryParse(Console.ReadLine(), out int numberOfGuests))
                    {
                        Console.WriteLine("Invalid number of guests.");
                        break;
                    }

                    Console.Write("Enter Total Price: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal totalPrice))
                    {
                        Console.WriteLine("Invalid Total Price.");
                        break;
                    }

                    //Console.Write("Enter Status: ");
                    //string status = Console.ReadLine();

                    // Add the new booking
                    await NewBooking.AddNewBooking(customerId, accommodationId, startDate, endDate, totalPrice, numberOfGuests);
                    Console.WriteLine("\nBOOKING COMPLETED! \n Don't forget to add the guests into the booking.");
                    break;

                case "2":
                    CancelBooking.AllBookings();
                    
                    Console.WriteLine("Write the id of the booking you would like to cancel: ");
                    if (int.TryParse(Console.ReadLine(), out int cancelId))
                    {
                        CancelBooking.DeleteBooking(cancelId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }
                    break;
                
                case "3":
                    // Placeholder for Change Booking functionality
                    Console.WriteLine("Change Booking functionality coming soon...");
                    break;
                case "4":
                   NewBooking.FilterPrice();
                    break;
                
                case "5":
                    NewBooking.FilterReview();
                    break;
                
                case "6":
                    Console.WriteLine("Please enter guest's details");
                    await _customerManager.AddGuestAsync();
                    break;
                
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task ManageCustomers()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("------- Manage Customers --------");
            Console.WriteLine("\n1. View All Customers" +
                              "\n2. View customer's details" +
                              "\n3. Add customer" +
                              "\n4. Back to Main Menu");
            Console.Write("\nChoose an option: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.ForegroundColor = ConsoleColor.Red;
                    DatabaseQueries.GetAllCustomers();
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    break;
                case "2":
                    Console.WriteLine("\nEnter customer details (e.g., Name, Email, Phone):");
                    DatabaseQueries.GetAllCustomersEmail();
                    Console.WriteLine("Add Customer functionality coming soon...");
                    Console.ReadKey();
                    break;
                case "3":
                    Console.WriteLine("\nEnter customer details (e.g., Name, Email, Phone):");
                    await _customerManager.AddCustomerAsync();
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
        private async Task ManageAccommodations()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("------- Manage Accommodations --------");
                Console.WriteLine("\n1. Search Available Accommodations" +
                                  "\n2. Back to Main Menu");
                Console.Write("\nChoose an option: ");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await SearchAvailableAccommodationsAsync();
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

        private async Task SearchAvailableAccommodationsAsync()
        {
            // Collect search criteria from the user
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

            int? locationId = null;
            Console.Write("Enter Location ID (or leave blank): ");
            string? locationInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(locationInput) && int.TryParse(locationInput, out int locId))
                locationId = locId;

            Console.Write("Enter Minimum Price (or leave blank): ");
            decimal? minPrice = null;
            string minPriceInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(minPriceInput) && decimal.TryParse(minPriceInput, out decimal minPriceValue))
                minPrice = minPriceValue;

            Console.Write("Enter Maximum Price (or leave blank): ");
            decimal? maxPrice = null;
            string maxPriceInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(maxPriceInput) && decimal.TryParse(maxPriceInput, out decimal maxPriceValue))
                maxPrice = maxPriceValue;

            // Create an instance of AccommodationManager
            var accommodationManager = new AccommodationManager(db.Connection());

            // Search for available accommodations
            await accommodationManager.SearchAvailableAccommodationsAsync(startDate, endDate, locationId, minPrice, maxPrice);

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }
using System.Diagnostics;
using alcocodebnb.BookingQueries;
using alcocodebnb.CustomerQueries;

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
                    ManageAccommodations();
                    break;
                case "4":
                    Console.WriteLine("Goodbye!");
                    return;
                case "5":
                    Console.WriteLine("\nChoose an option:" +
                                      "\n1. Run Synchronous Operation" +
                                      "\n2. Run Asynchronous Operation");
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
                            Console.WriteLine("Starting asynchronous operation...");
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

    private void ManageBookings()
    {
        while (true)
        {
            //Console.Clear();
            Console.WriteLine("------- Manage Bookings --------");
            Console.WriteLine("\n1. New Booking" +
                              "\n2. Cancel Booking" +
                              "\n3. Change Booking" +
                              "\n4. Filter price" +
                              "\n5. Filter review" +
                              "\n6. Back to Main Menu");
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

                    Console.Write("Enter Status: ");
                    //string status = Console.ReadLine();

                    // Add the new booking
                    NewBooking.AddNewBooking(customerId, accommodationId, startDate, endDate, totalPrice, numberOfGuests);
                    Console.WriteLine("\nBOOKING COMPLETED! \nPress any key to exit.");
                    Console.ReadKey();
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
                    DatabaseQueries.GetAllCustomers();
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

    private void ManageAccommodations()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("------- Manage Accommodations --------");
            Console.WriteLine("\n1. View All Accommodations" +
                              "\n2. Search Accommodations" +
                              "\n3. Back to Main Menu");
            Console.Write("\nChoose an option: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    // Placeholder for View All Accommodations functionality
                    Console.WriteLine("View All Accommodations functionality coming soon...");
                    Console.ReadKey();
                    break;
                case "2":
                    // Placeholder for Search Accommodations functionality
                    Console.WriteLine("Search Accommodations functionality coming soon...");
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
}
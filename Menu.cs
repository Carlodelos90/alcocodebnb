using System.Runtime.InteropServices.JavaScript;
using alcocodebnb.BookingQueries;

namespace alcocodebnb;

public class Menu
{
    public Menu()
    {
        DatabaseConnection db = new();
        DatabaseQueries queries = new(db.Connection());
        CancelBooking cancel = new(db.Connection());
        NewBooking addBooking = new(db.Connection());
        EditBooking editBooking = new(db.Connection());
    }

    public void Start()
    {
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("------- Welcome to AlcoCodeBNB --------");
            Console.WriteLine("\n1. Manage Bookings" +
                              "\n2. Customers" +
                              "\n3. Accommodations" +
                              "\n4. Exit");
            Console.Write("\nChoose an option: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ManageBookings();
                    break;
                case "2":
                    ManageCustomers();
                    break;
                case "3":
                    ManageAccommodations();
                    break;
                case "4":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task ManageBookings()
    {
        Console.Clear();
        while (true)
        {
            //Console.Clear();
            Console.WriteLine("------- Manage Bookings --------");
            Console.WriteLine("\n1. New Booking" +
                              "\n2. Cancel Booking" +
                              "\n3. Change Booking" +
                              "\n4. Back to Main Menu");
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
                    if (int.TryParse(Console.ReadLine(), out int CancelId))
                    {
                        CancelBooking.DeleteBooking(CancelId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }
                    break;
                    Console.WriteLine("Cancel Booking functionality coming soon...");
                    break;
                case "3":
                    while (true)
{
    Console.Clear();
    Console.WriteLine("------- Edit Bookings --------");
    Console.WriteLine("\n1. Change the date of your booking" +
                      "\n2. Change the number of guests" +
                      "\n3. Change extra addons" +
                      "\n4. Exit");
    Console.Write("\nChoose an option: ");

    string? inputEdit = Console.ReadLine();

    switch (inputEdit)
    {
        case "1":
            Console.Clear();
            EditBooking.GetAllBookingsAsync();

            Console.Write("Enter the booking ID: ");
            if (int.TryParse(Console.ReadLine(), out int bookingId))
            {
                Console.Write("Enter the new start date (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime startdate))
                {
                    Console.Write("Enter the new end date (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime enddate))
                    {
                        await EditBooking.ChangeBookingDateAsync(bookingId, startdate, enddate);
                        Console.WriteLine("Booking date updated successfully!");
                    }
                    else
                        Console.WriteLine("Invalid end date.");
                }
                else
                    Console.WriteLine("Invalid start date.");
            }
            else
                Console.WriteLine("Invalid booking ID.");
            break;

        case "2":
            Console.Clear();
            EditBooking.GetAllBookingsAsync();
            Console.Write("Enter the booking ID: ");
            if (int.TryParse(Console.ReadLine(), out int guestBookingId))
            {
                Console.Write("Enter the new number of guests: ");
                if (int.TryParse(Console.ReadLine(), out int numberofguests))
                {
                    await EditBooking.ChangeGuestsAsync(guestBookingId, numberofguests);
                    Console.WriteLine("Number of guests updated successfully!");
                }
                else
                    Console.WriteLine("Invalid number of guests.");
            }
            else
                Console.WriteLine("Invalid booking ID.");
            break;

        case "3":
            Console.Clear();
            EditBooking.GetAllAddonsAsync();
            Console.Write("Enter the booking ID: ");
            if (int.TryParse(Console.ReadLine(), out int extrabookingid))
            {
                Console.Write("Enter the extra service ID: ");
                if (int.TryParse(Console.ReadLine(), out int extraserviceid))
                {
                    Console.Write("Enter the quantity: ");
                    if (int.TryParse(Console.ReadLine(), out int quantity))
                    {
                        await EditBooking.ChangeBoardOptionAsync(extraserviceid, quantity, extrabookingid);
                        Console.WriteLine("Extra addons updated successfully!");
                    }
                    else
                        Console.WriteLine("Invalid quantity.");
                }
                else
                    Console.WriteLine("Invalid extra service ID.");
            }
            else
                Console.WriteLine("Invalid booking ID.");
            break;

        case "4":
            Console.WriteLine("Exiting... Goodbye!");
            return;

        default:
            Console.WriteLine("Invalid option. Press any key to try again.");
            Console.ReadKey();
            break;
    }

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}


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

    private void ManageCustomers()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("------- Manage Customers --------");
            Console.WriteLine("\n1. View All Customers" +
                              "\n2. View customer's details" +
                              "\n3. Back to Main Menu");
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
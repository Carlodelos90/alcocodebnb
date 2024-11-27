using alcocodebnb.BookingQueries;

namespace alcocodebnb;

public class Menu
{
    private readonly DatabaseConnection _dbConnection;
    private readonly DatabaseQueries _dbQueries;

    public Menu()
    {
        _dbConnection = new DatabaseConnection();
        _dbQueries = new DatabaseQueries(_dbConnection);
        

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

    private void ManageBookings()
    {
        //All the connections needed
        var cancelBooking = new CancelBooking(_dbConnection);
        
        while (true)
        {
            //Console.Clear();
            Console.WriteLine("------- Manage Bookings --------");
            Console.WriteLine("\n1. New Booking" +
                              "\n2. Cancel Booking" +
                              "\n3. Change Booking" +
                              "\n4. Back to Main Menu");
            Console.Write("\nChoose an option: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    cancelBooking.ShowBookings();
                    Console.WriteLine("New Booking functionality coming soon...");
                    break;
                case "2":
                    // Placeholder for Cancel booking functionality
                    Console.WriteLine("Cancel Booking functionality coming soon...");
                    break;
                case "3":
                    // Placeholder for Change Booking functionality
                    Console.WriteLine("Change Booking functionality coming soon...");
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
                              "\n2. Add New Customer" +
                              "\n3. Back to Main Menu");
            Console.Write("\nChoose an option: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    _dbQueries.GetAllCustomers();
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    break;
                case "2":
                    Console.WriteLine("\nEnter customer details (e.g., Name, Email, Phone):");
                    // Placeholder for adding customer functionality
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
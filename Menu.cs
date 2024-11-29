using System.Runtime.InteropServices.JavaScript;
using alcocodebnb.BookingQueries;
using Npgsql;

namespace alcocodebnb;

public class Menu
{
    private readonly DatabaseConnection _dbConnection;
    private readonly DatabaseQueries _dbQueries;

    public Menu()
    {
        DatabaseConnection db = new();
        DatabaseQueries queries = new(db.Connection());
        CancelBooking cancel = new(db.Connection());
        NewBooking addBooking = new(db.Connection());

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
        //var cancelBooking = new CancelBooking(_database);
        
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
                    Console.Clear();
                    NewBooking.AllLocations();
                    Console.WriteLine("Enter the id of the location you would like to book: ");
                    /*
                    if (int.TryParse(Console.ReadLine(), out int NewBookingId))
                    {
                        NewBooking.AddNewBooking(NewBookingId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }*/
                    
                    
                    //Console.WriteLine("New Booking functionality coming soon...");
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

                    // Placeholder for Cancel booking functionality
                    Console.WriteLine("Cancel Booking functionality coming soon...");
                    break;
                case "3":
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("------- Edit Bookings --------");
                        Console.WriteLine("\n1. Change the date of your booking: " +
                                          "\n2. Change the amount of guests: " +
                                          "\n3. Change extra addons" +
                                          "\n4. Exit");
                        Console.Write("\nChoose an option: ");

                        string? inputEdit = Console.ReadLine();

                        switch (inputEdit)
                        {
                            case "1":
                                Console.WriteLine("What is the id of the booking you would like to change?: ");
                                int EditId = Convert.ToInt32(Console.ReadLine());
                                Console.WriteLine("Start Date: ");
                                DateTime startdate = Convert.ToDateTime(Console.ReadLine());
                                Console.WriteLine("End Date: ");
                                DateTime enddate = Convert.ToDateTime(Console.ReadLine());
                                EditBooking.ChangeBookingDate(EditId, startdate, enddate);
                                break;

                            case "2":
                                Console.WriteLine("Do you want to add or remove guests?");
                                string addOrRemove = Console.ReadLine();
                                if (addOrRemove == "add")
                                {
                                    

                                }
                                else if (addOrRemove == "remove")
                                {

                                }
                                break;
                            
                            case "3":
                                


                            case "4":
                                return;

                            default:
                                Console.WriteLine("Invalid option. Press any key to try again.");
                                Console.ReadKey();
                                break;
                        }
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
                              "\n2. Add New Customer" +
                              "\n3. Back to Main Menu");
            Console.Write("\nChoose an option: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    //GetAllCustomers();
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
namespace alcocodebnb;

public class Menu
{
    public void Start()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("------- Welcome to AlcoCodeBNB --------");
            Console.WriteLine("\n1. Manage Bookings" +
                              "\n2. Customers" +
                              "\n3. Accomodations" +
                              "\n4. Exit");
            Console.Write("Choose an option: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ManageBookings();
                    break;
                case "2":
                    //Customer();
                    break;
                case "3":
                    //Accomodatoin();
                    break;
                case "4":
                    exit = true;
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    public void ManageBookings()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();

            Console.WriteLine("------- What do you want to do? --------");
            Console.WriteLine("\n1. New Booking" +
                              "\n2. Cancel Booking" +
                              "\n3. Change Booking" +
                              "\n4. Exit");
            Console.Write("Choose an option: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    //NewBooking();
                    break;
                case "2":
                    //CancelBooking();
                    break;
                case "3":
                    //ChangeBooking();
                    break;
                case "4":
                    exit = true;
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}
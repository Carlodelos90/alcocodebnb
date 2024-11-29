

namespace alcocodebnb
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Menu menu = new Menu();
                menu.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}




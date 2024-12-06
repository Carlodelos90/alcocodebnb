using alcocodebnb.BookingQueries;
using alcocodebnb.CustomerQueries;
using alcocodebnb.AccommodationQueries;

namespace alcocodebnb
{
    class Program
    {
        static readonly DatabaseConnection Db = new();
        static async Task Main(string[] args)
        {
            Menu menu = new();
            await menu.Start();
        }
    }
}




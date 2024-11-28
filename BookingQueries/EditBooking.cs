namespace alcocodebnb.BookingQueries;
using Npgsql;

public class EditBooking
{
    private NpgsqlDataSource _database;

    public EditBooking(NpgsqlDataSource database)
    {
        _database = database;
    }
    public async void UpdateItem(int id, String name, double price, int stock)
    {
        await using (var cmd = _database.CreateCommand("UPDATE booking " +
                                                       "SET name = $1, price = $2, stock = $3 " +
                                                       " WHERE id = $4"))
        {
            cmd.Parameters.AddWithValue(name);
            cmd.Parameters.AddWithValue(price);
            cmd.Parameters.AddWithValue(stock);
            cmd.Parameters.AddWithValue(id);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
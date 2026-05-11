using System.Text;
using Microsoft.Data.Sqlite;

/// <summary>
/// Одна строка результата SQL-запроса.
/// </summary>
record CsvRow(string[] Fields);

/// <summary>
/// Таблица результата SQL-запроса: заголовки и строки.
/// </summary>
record CsvTable(string[] Headers, List<CsvRow> Rows);

/// <summary>
/// Класс для работы с SQLite-базой данных ресторанов и блюд.
/// </summary>
class DatabaseManager
{
    private readonly string _connectionString;

    /// <summary>
    /// Создаёт менеджер базы данных и таблицы, если они отсутствуют.
    /// </summary>
    public DatabaseManager(string databasePath)
    {
        _connectionString = $"Data Source={databasePath}";
        CreateTables();
    }

    /// <summary>
    /// Создаёт таблицы restaurants и menu_items, если они ещё не созданы.
    /// </summary>
    public void CreateTables()
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();

        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS restaurants (
                restaurant_id INTEGER PRIMARY KEY,
                restaurant_name TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS menu_items (
                dish_id INTEGER PRIMARY KEY,
                restaurant_id INTEGER NOT NULL,
                dish_name TEXT NOT NULL,
                price INTEGER NOT NULL,
                FOREIGN KEY (restaurant_id) REFERENCES restaurants(restaurant_id)
            );";

        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Инициализирует базу CSV-данными, если таблицы пустые.
    /// </summary>
    public void InitializeDatabase(string restaurantsPath, string menuItemsPath)
    {
        if (GetTableCount("restaurants") == 0 && GetTableCount("menu_items") == 0)
            ImportFromCsv(restaurantsPath, menuItemsPath);
    }

    /// <summary>
    /// Очищает таблицы и импортирует данные из CSV-файлов.
    /// </summary>
    public void ImportFromCsv(string restaurantsPath, string menuItemsPath)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteTransaction transaction = connection.BeginTransaction();

        using (SqliteCommand command = connection.CreateCommand())
        {
            command.Transaction = transaction;
            command.CommandText = "DELETE FROM menu_items; DELETE FROM restaurants;";
            command.ExecuteNonQuery();
        }

        ImportRestaurants(connection, transaction, restaurantsPath);
        ImportMenuItems(connection, transaction, menuItemsPath);

        transaction.Commit();
    }

    /// <summary>
    /// Возвращает список всех ресторанов.
    /// </summary>
    public List<Restaurant> GetAllRestaurants()
    {
        var result = new List<Restaurant>();

        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT restaurant_id, restaurant_name FROM restaurants ORDER BY restaurant_id;";

        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new Restaurant(reader.GetInt32(0), reader.GetString(1)));
        }

        return result;
    }

    /// <summary>
    /// Возвращает список всех блюд.
    /// </summary>
    public List<MenuItem> GetAllMenuItems()
    {
        var result = new List<MenuItem>();

        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT dish_id, restaurant_id, dish_name, price FROM menu_items ORDER BY dish_id;";

        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new MenuItem(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetString(2),
                reader.GetInt32(3)));
        }

        return result;
    }

    /// <summary>
    /// Возвращает блюдо по идентификатору или null, если блюдо не найдено.
    /// </summary>
    public MenuItem? GetMenuItemById(int id)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
            SELECT dish_id, restaurant_id, dish_name, price
            FROM menu_items
            WHERE dish_id = $id;";
        command.Parameters.AddWithValue("$id", id);

        using SqliteDataReader reader = command.ExecuteReader();
        if (!reader.Read())
            return null;

        return new MenuItem(
            reader.GetInt32(0),
            reader.GetInt32(1),
            reader.GetString(2),
            reader.GetInt32(3));
    }

    /// <summary>
    /// Добавляет новое блюдо в меню.
    /// </summary>
    public void AddMenuItem(MenuItem item)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();

        if (item.Id > 0)
        {
            command.CommandText = @"
                INSERT INTO menu_items (dish_id, restaurant_id, dish_name, price)
                VALUES ($id, $restaurantId, $name, $price);";
            command.Parameters.AddWithValue("$id", item.Id);
        }
        else
        {
            command.CommandText = @"
                INSERT INTO menu_items (restaurant_id, dish_name, price)
                VALUES ($restaurantId, $name, $price);";
        }

        command.Parameters.AddWithValue("$restaurantId", item.RestaurantId);
        command.Parameters.AddWithValue("$name", item.Name);
        command.Parameters.AddWithValue("$price", item.Price);
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Обновляет данные блюда по идентификатору.
    /// </summary>
    public void UpdateMenuItem(MenuItem item)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE menu_items
            SET restaurant_id = $restaurantId,
                dish_name = $name,
                price = $price
            WHERE dish_id = $id;";
        command.Parameters.AddWithValue("$id", item.Id);
        command.Parameters.AddWithValue("$restaurantId", item.RestaurantId);
        command.Parameters.AddWithValue("$name", item.Name);
        command.Parameters.AddWithValue("$price", item.Price);
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Удаляет блюдо по идентификатору.
    /// </summary>
    public void DeleteMenuItem(int id)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "DELETE FROM menu_items WHERE dish_id = $id;";
        command.Parameters.AddWithValue("$id", id);
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Выполняет SQL-запрос и возвращает таблицу строк для отчётов.
    /// </summary>
    public CsvTable ExecuteQuery(string sql)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = sql;

        using SqliteDataReader reader = command.ExecuteReader();
        string[] headers = new string[reader.FieldCount];
        for (int i = 0; i < reader.FieldCount; i++)
            headers[i] = reader.GetName(i);

        var rows = new List<CsvRow>();
        while (reader.Read())
        {
            string[] fields = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
                fields[i] = reader.GetValue(i).ToString() ?? "";

            rows.Add(new CsvRow(fields));
        }

        return new CsvTable(headers, rows);
    }

    /// <summary>
    /// Экспортирует текущие данные таблиц в CSV-файлы.
    /// </summary>
    public void ExportToCsv(string restaurantsPath, string menuItemsPath)
    {
        string? restaurantsDirectory = Path.GetDirectoryName(restaurantsPath);
        if (!string.IsNullOrEmpty(restaurantsDirectory))
            Directory.CreateDirectory(restaurantsDirectory);

        string? menuItemsDirectory = Path.GetDirectoryName(menuItemsPath);
        if (!string.IsNullOrEmpty(menuItemsDirectory))
            Directory.CreateDirectory(menuItemsDirectory);

        using (var writer = new StreamWriter(restaurantsPath, false, Encoding.UTF8))
        {
            writer.WriteLine("restaurant_id;restaurant_name");
            List<Restaurant> restaurants = GetAllRestaurants();
            for (int i = 0; i < restaurants.Count; i++)
            {
                Restaurant restaurant = restaurants[i];
                writer.WriteLine($"{restaurant.Id};{EscapeCsv(restaurant.Name)}");
            }
        }

        using (var writer = new StreamWriter(menuItemsPath, false, Encoding.UTF8))
        {
            writer.WriteLine("dish_id;restaurant_id;dish_name;price");
            List<MenuItem> items = GetAllMenuItems();
            for (int i = 0; i < items.Count; i++)
            {
                MenuItem item = items[i];
                writer.WriteLine($"{item.Id};{item.RestaurantId};{EscapeCsv(item.Name)};{item.Price}");
            }
        }
    }

    private SqliteConnection OpenConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = ON;";
        command.ExecuteNonQuery();

        return connection;
    }

    private int GetTableCount(string tableName)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"SELECT COUNT(*) FROM {tableName};";
        return Convert.ToInt32(command.ExecuteScalar());
    }

    private void ImportRestaurants(SqliteConnection connection, SqliteTransaction transaction, string path)
    {
        string[] lines = File.ReadAllLines(path, Encoding.UTF8);
        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i].Trim().Length == 0)
                continue;

            string[] parts = lines[i].Split(';');
            if (parts.Length < 2)
                continue;

            using SqliteCommand command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = @"
                INSERT INTO restaurants (restaurant_id, restaurant_name)
                VALUES ($id, $name);";
            command.Parameters.AddWithValue("$id", int.Parse(parts[0]));
            command.Parameters.AddWithValue("$name", parts[1]);
            command.ExecuteNonQuery();
        }
    }

    private void ImportMenuItems(SqliteConnection connection, SqliteTransaction transaction, string path)
    {
        string[] lines = File.ReadAllLines(path, Encoding.UTF8);
        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i].Trim().Length == 0)
                continue;

            string[] parts = lines[i].Split(';');
            if (parts.Length < 4)
                continue;

            var item = new MenuItem(
                int.Parse(parts[0]),
                int.Parse(parts[1]),
                parts[2],
                int.Parse(parts[3]));

            using SqliteCommand command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = @"
                INSERT INTO menu_items (dish_id, restaurant_id, dish_name, price)
                VALUES ($id, $restaurantId, $name, $price);";
            command.Parameters.AddWithValue("$id", item.Id);
            command.Parameters.AddWithValue("$restaurantId", item.RestaurantId);
            command.Parameters.AddWithValue("$name", item.Name);
            command.Parameters.AddWithValue("$price", item.Price);
            command.ExecuteNonQuery();
        }
    }

    private string EscapeCsv(string value)
    {
        return value.Replace(";", ",");
    }
}

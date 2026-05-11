using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

string projectDirectory = GetProjectDirectory();
string dbPath = Path.Combine(projectDirectory, "restaurants.db");
string restaurantsCsv = Path.Combine(projectDirectory, "restaurants.csv");
string menuItemsCsv = Path.Combine(projectDirectory, "menu_items.csv");
string resultsDirectory = Path.Combine(projectDirectory, "results");

Directory.CreateDirectory(resultsDirectory);

var db = new DatabaseManager(dbPath);
db.InitializeDatabase(restaurantsCsv, menuItemsCsv);

string choice;
do
{
    Console.WriteLine("========================================");
    Console.WriteLine("       МЕНЮ РЕСТОРАНОВ И БЛЮД");
    Console.WriteLine("========================================");
    Console.WriteLine("1 — Показать все рестораны");
    Console.WriteLine("2 — Показать все блюда");
    Console.WriteLine("3 — Добавить блюдо");
    Console.WriteLine("4 — Редактировать блюдо");
    Console.WriteLine("5 — Удалить блюдо");
    Console.WriteLine("6 — Отчёты");
    Console.WriteLine("7 — Экспорт в CSV");
    Console.WriteLine("0 — Выход");
    Console.Write("Ваш выбор: ");

    choice = Console.ReadLine()?.Trim() ?? "0";
    Console.WriteLine();

    try
    {
        switch (choice)
        {
            case "1":
                ShowRestaurants(db);
                break;
            case "2":
                ShowMenuItems(db);
                break;
            case "3":
                AddMenuItem(db);
                break;
            case "4":
                EditMenuItem(db);
                break;
            case "5":
                DeleteMenuItem(db);
                break;
            case "6":
                ReportsMenu(db, resultsDirectory);
                break;
            case "7":
                ExportCsv(db, resultsDirectory);
                break;
            case "0":
                break;
            default:
                Console.WriteLine("Неверный пункт меню.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }

    Console.WriteLine();
}
while (choice != "0");

static string GetProjectDirectory()
{
    string currentDirectory = Directory.GetCurrentDirectory();

    if (File.Exists(Path.Combine(currentDirectory, "restaurants.csv")))
        return currentDirectory;

    string homeworkDirectory = Path.Combine(currentDirectory, "Homework-2");
    if (File.Exists(Path.Combine(homeworkDirectory, "restaurants.csv")))
        return homeworkDirectory;

    return AppContext.BaseDirectory;
}

static void ShowRestaurants(DatabaseManager db)
{
    Console.WriteLine("--- Все рестораны ---");
    List<Restaurant> restaurants = db.GetAllRestaurants();
    int[] widths = new int[] { 6, 33 };

    PrintTableLine(widths);
    PrintTableRow(new string[] { "ID", "Название ресторана" }, widths);
    PrintTableLine(widths);

    for (int i = 0; i < restaurants.Count; i++)
    {
        Restaurant restaurant = restaurants[i];
        PrintTableRow(
            new string[] { restaurant.Id.ToString(), restaurant.Name },
            widths);
    }

    PrintTableLine(widths);
    Console.WriteLine($"Итого: {restaurants.Count}");
}

static void ShowMenuItems(DatabaseManager db)
{
    Console.WriteLine("--- Все блюда ---");
    List<MenuItem> items = db.GetAllMenuItems();
    int[] widths = new int[] { 6, 10, 40, 14 };

    PrintTableLine(widths);
    PrintTableRow(
        new string[] { "ID", "Ресторан", "Блюдо", "Цена, руб." },
        widths);
    PrintTableLine(widths);

    for (int i = 0; i < items.Count; i++)
    {
        MenuItem item = items[i];
        PrintTableRow(
            new string[] { item.Id.ToString(), item.RestaurantId.ToString(), item.Name, item.Price.ToString() },
            widths);
    }

    PrintTableLine(widths);
    Console.WriteLine($"Итого: {items.Count}");
}

static void PrintTableLine(int[] widths)
{
    int width = 1;
    for (int i = 0; i < widths.Length; i++)
        width += widths[i] + 3;

    Console.WriteLine("+" + new string('-', width - 2) + "+");
}

static void PrintTableRow(string[] values, int[] widths)
{
    Console.Write("|");
    for (int i = 0; i < widths.Length; i++)
    {
        string value = "";
        if (i < values.Length)
            value = values[i];

        Console.Write(" " + Cut(value, widths[i]).PadRight(widths[i]) + " |");
    }
    Console.WriteLine();
}

static string Cut(string value, int width)
{
    if (value.Length <= width)
        return value;

    if (width <= 3)
        return value.Substring(0, width);

    return value.Substring(0, width - 3) + "...";
}

static void AddMenuItem(DatabaseManager db)
{
    Console.WriteLine("--- Добавление блюда ---");
    Console.WriteLine("Доступные рестораны:");
    ShowRestaurants(db);

    Console.Write("ID ресторана: ");
    if (!int.TryParse(Console.ReadLine(), out int restaurantId))
    {
        Console.WriteLine("Ошибка: введите целое число.");
        return;
    }

    Console.Write("Название блюда: ");
    string name = Console.ReadLine()?.Trim() ?? "";
    if (name.Length == 0)
    {
        Console.WriteLine("Ошибка: название блюда не может быть пустым.");
        return;
    }

    Console.Write("Цена в рублях: ");
    if (!int.TryParse(Console.ReadLine(), out int price))
    {
        Console.WriteLine("Ошибка: введите целое число.");
        return;
    }

    try
    {
        var item = new MenuItem(0, restaurantId, name, price);
        db.AddMenuItem(item);
        Console.WriteLine("Блюдо добавлено.");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    }
}

static void EditMenuItem(DatabaseManager db)
{
    Console.WriteLine("--- Редактирование блюда ---");
    Console.Write("Введите ID блюда: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Ошибка: введите целое число.");
        return;
    }

    MenuItem? item = db.GetMenuItemById(id);
    if (item == null)
    {
        Console.WriteLine($"Блюдо с ID={id} не найдено.");
        return;
    }

    Console.WriteLine($"Текущие данные: {item}");
    Console.WriteLine("Нажмите Enter, чтобы оставить старое значение.");

    Console.Write($"ID ресторана [{item.RestaurantId}]: ");
    string input = Console.ReadLine()?.Trim() ?? "";
    if (input.Length > 0)
    {
        if (!int.TryParse(input, out int restaurantId))
        {
            Console.WriteLine("Ошибка: ID ресторана должен быть целым числом.");
            return;
        }
        item.RestaurantId = restaurantId;
    }

    Console.Write($"Название блюда [{item.Name}]: ");
    input = Console.ReadLine()?.Trim() ?? "";
    if (input.Length > 0)
        item.Name = input;

    Console.Write($"Цена [{item.Price}]: ");
    input = Console.ReadLine()?.Trim() ?? "";
    if (input.Length > 0)
    {
        if (!int.TryParse(input, out int price))
        {
            Console.WriteLine("Ошибка: цена должна быть целым числом.");
            return;
        }

        try
        {
            item.Price = price;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            return;
        }
    }

    db.UpdateMenuItem(item);
    Console.WriteLine("Данные обновлены.");
}

static void DeleteMenuItem(DatabaseManager db)
{
    Console.WriteLine("--- Удаление блюда ---");
    Console.Write("Введите ID блюда: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Ошибка: введите целое число.");
        return;
    }

    MenuItem? item = db.GetMenuItemById(id);
    if (item == null)
    {
        Console.WriteLine($"Блюдо с ID={id} не найдено.");
        return;
    }

    Console.Write($"Удалить «{item.Name}»? (да/нет): ");
    string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";
    if (confirm == "да")
    {
        db.DeleteMenuItem(id);
        Console.WriteLine("Блюдо удалено.");
    }
    else
    {
        Console.WriteLine("Удаление отменено.");
    }
}

static void ReportsMenu(DatabaseManager db, string resultsDirectory)
{
    string choice;
    do
    {
        Console.WriteLine("--- Отчёты ---");
        Console.WriteLine("1 — Полный список блюд с ресторанами");
        Console.WriteLine("2 — Количество блюд в каждом ресторане");
        Console.WriteLine("3 — Средняя цена блюд по ресторанам");
        Console.WriteLine("0 — Назад");
        Console.Write("Ваш выбор: ");

        choice = Console.ReadLine()?.Trim() ?? "0";
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                Report1(db, resultsDirectory);
                break;
            case "2":
                Report2(db, resultsDirectory);
                break;
            case "3":
                Report3(db, resultsDirectory);
                break;
            case "0":
                break;
            default:
                Console.WriteLine("Неверный пункт.");
                break;
        }

        Console.WriteLine();
    }
    while (choice != "0");
}

static void Report1(DatabaseManager db, string resultsDirectory)
{
    string path = Path.Combine(resultsDirectory, "report1.txt");

    var builder = new ReportBuilder(db)
        .Query(@"SELECT m.dish_name, r.restaurant_name, m.price
                 FROM menu_items m
                 JOIN restaurants r ON m.restaurant_id = r.restaurant_id
                 ORDER BY m.dish_name;")
        .Title("Полный список блюд с ресторанами")
        .Header("Блюдо", "Ресторан", "Цена")
        .ColumnWidths(25, 25, 10);

    builder.Print();
    builder.SaveToFile(path);
}

static void Report2(DatabaseManager db, string resultsDirectory)
{
    string path = Path.Combine(resultsDirectory, "report2.txt");

    var builder = new ReportBuilder(db)
        .Query(@"SELECT r.restaurant_name, COUNT(*) AS cnt
                 FROM menu_items m
                 JOIN restaurants r ON m.restaurant_id = r.restaurant_id
                 GROUP BY r.restaurant_name
                 ORDER BY r.restaurant_name;")
        .Title("Количество блюд в каждом ресторане")
        .Header("Ресторан", "Количество")
        .ColumnWidths(25, 12);

    builder.Print();
    builder.SaveToFile(path);
}

static void Report3(DatabaseManager db, string resultsDirectory)
{
    string path = Path.Combine(resultsDirectory, "report3.txt");

    var builder = new ReportBuilder(db)
        .Query(@"SELECT r.restaurant_name, ROUND(AVG(m.price), 1) AS avg_price
                 FROM menu_items m
                 JOIN restaurants r ON m.restaurant_id = r.restaurant_id
                 GROUP BY r.restaurant_name
                 ORDER BY avg_price DESC;")
        .Title("Средняя цена блюд по ресторанам")
        .Header("Ресторан", "Средняя цена")
        .ColumnWidths(25, 15);

    builder.Print();
    builder.SaveToFile(path);
}

static void ExportCsv(DatabaseManager db, string resultsDirectory)
{
    string restaurantsPath = Path.Combine(resultsDirectory, "restaurants_export.csv");
    string menuItemsPath = Path.Combine(resultsDirectory, "menu_items_export.csv");

    db.ExportToCsv(restaurantsPath, menuItemsPath);
    Console.WriteLine($"Рестораны экспортированы в: {restaurantsPath}");
    Console.WriteLine($"Блюда экспортированы в: {menuItemsPath}");
}

Catalog<LibraryItem> catalog = new("Расширенный каталог");

try
{
    catalog.Add(new Book("Мастер и Маргарита", "М. Булгаков", 1967, BookGenre.Novel, ItemStatus.Available));
    catalog.Add(new Book("1984", "Дж. Оруэлл", 1949, BookGenre.SciFi, ItemStatus.CheckedOut));
    catalog.Add(new Magazine("Наука и жизнь", 2024, 5, ItemStatus.Available));
    catalog.Add(new Magazine("Исторический вестник", 1910, 2, ItemStatus.Archived));

    Console.WriteLine("Каталог после добавления тестовых данных:");
    catalog.PrintAll();

    Console.WriteLine();
    Console.WriteLine("Поиск по слову 'мастер' в названии и авторе:");
    foreach (LibraryItem item in catalog.Search("мастер", SearchOptions.Title | SearchOptions.Author))
    {
        Console.WriteLine($"  {item.GetInfo()}");
    }

    Console.WriteLine();
    Console.WriteLine("Старые элементы старше 70 лет:");
    foreach (LibraryItem item in catalog.FindOlderThan(70))
    {
        Console.WriteLine($"  {item.GetInfo()}");
    }

    Console.WriteLine();
    Console.WriteLine("CSV экспорт:");
    Console.WriteLine(catalog.ExportCsv());

    Console.WriteLine("JSON экспорт:");
    Console.WriteLine(catalog.ExportJson());

    Console.WriteLine();
    Console.WriteLine("Попытка добавить книгу из консоли:");
    catalog.AddFromConsole();
}
catch (InvalidBookDataException ex) when (ex.FieldName == nameof(Book.Year))
{
    Console.Error.WriteLine($"Ошибка года издания: {ex.Message}");
}
catch (LibraryException ex)
{
    Console.Error.WriteLine($"Ошибка библиотеки: {ex.Message}");
}
finally
{
    Console.WriteLine();
    Console.WriteLine("Работа с каталогом завершена.");
}

Console.WriteLine($"{LibraryUtils.LibraryName}");
Console.WriteLine($"Время запуска: {LibraryUtils.StartupTime:dd.MM.yyyy HH:mm:ss}");
LibraryUtils.PrintSeparator();

Book defaultBook = new()
{
    Title = "Без названия",
    Author = "Неизвестный автор",
    Genre = "Не указан"
};

Book classicBook = new("Мастер и Маргарита", "М. Булгаков", 1967, 480, "Роман");

Book initBook = new()
{
    Title = "1984",
    Author = "Дж. Оруэлл",
    Year = 1949,
    PageCount = 328,
    Genre = "Антиутопия"
};

Book[] books = [defaultBook, classicBook, initBook];

Console.WriteLine("Список книг:");
Console.WriteLine(LibraryUtils.FormatBookList(books));

LibraryUtils.PrintSeparator();
Console.WriteLine("Демонстрация методов Book:");
Console.WriteLine(classicBook.GetInfo());
Console.WriteLine(classicBook.GetInfo(showPages: false));
Console.WriteLine($"Старше 50 лет: {classicBook.IsOlderThan()}");
Console.WriteLine(classicBook.GetFormattedInfo(format: "full"));

LibraryUtils.PrintSeparator();
Book oldest = LibraryUtils.FindOldest(books);
Console.WriteLine($"Самая старая книга: {oldest.ShortDescription}");
Book.PrintStatistics();

Console.WriteLine("ООП-версия библиотечной системы");
Console.WriteLine();

LibraryItem[] catalog =
[
    new Book("Мастер и Маргарита", 1967, "М. Булгаков", 480),
    new Magazine("Наука и жизнь", 2024, 5),
    new Book("1984", 1949, "Дж. Оруэлл", 328)
];

Console.WriteLine("Полиморфизм через массив LibraryItem[]:");
foreach (LibraryItem item in catalog)
{
    Console.WriteLine(item.GetInfo());
    Console.WriteLine(item.GetCardInfo());
    Console.WriteLine($"Описание: {item.Description}");
    Console.WriteLine();
}

Console.WriteLine("Проверки is и as:");
foreach (LibraryItem item in catalog)
{
    if (item is Book book)
    {
        Console.WriteLine($"is: книга '{book.Title}', автор {book.Author}");
    }

    Magazine? magazine = item as Magazine;
    if (magazine is not null)
    {
        Console.WriteLine($"as: журнал '{magazine.Title}', выпуск {magazine.IssueNumber}");
    }
}

Console.WriteLine();
Console.WriteLine("Методы расширения:");
foreach (LibraryItem item in catalog)
{
    item.PrintCard();
    Console.WriteLine($"CSV: {item.ToCsvLine()}");
    Console.WriteLine($"Новое издание: {(item.IsNew() ? "да" : "нет")}");
    Console.WriteLine();
}

Console.WriteLine("Partial Library + Fluent Interface:");
Library library = new Library()
    .Add(new Book("Преступление и наказание", 1866, "Ф. Достоевский", 672))
    .Add(new Magazine("Квант", 2023, 12))
    .Add(new Book("Солярис", 1961, "С. Лем", 256));

library.PrintAll();

Console.WriteLine();
Console.WriteLine("Поиск по названию 'с':");
foreach (LibraryItem item in library.FindByTitle("с"))
{
    Console.WriteLine($"  {item.GetInfo()}");
}

Console.WriteLine();
Console.WriteLine("Только книги:");
foreach (Book book in library.FindBooks())
{
    Console.WriteLine($"  {book.GetInfo()}");
}

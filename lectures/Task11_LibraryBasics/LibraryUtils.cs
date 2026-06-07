internal static class LibraryUtils
{
    public const string LibraryName = "Городская библиотека";

    public static readonly DateTime StartupTime = DateTime.Now;

    public static void PrintSeparator(char symbol = '-', int length = 40)
    {
        Console.WriteLine(new string(symbol, length));
    }

    public static string FormatBookList(Book[] books)
    {
        return string.Join(Environment.NewLine, books.Select((book, index) => $"{index + 1}. {book.GetInfo()}"));
    }

    public static Book FindOldest(Book[] books)
    {
        if (books.Length == 0)
        {
            throw new ArgumentException("Массив книг не должен быть пустым.", nameof(books));
        }

        Book oldest = books[0];

        foreach (Book book in books)
        {
            if (book.Year < oldest.Year)
            {
                oldest = book;
            }
        }

        return oldest;
    }
}

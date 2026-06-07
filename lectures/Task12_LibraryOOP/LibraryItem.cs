internal abstract class LibraryItem
{
    private static int _totalItems;

    protected LibraryItem(string title, int year)
    {
        Title = title;
        Year = year;
        _totalItems++;
        Console.WriteLine($"Вызван конструктор LibraryItem: {Title}");
    }

    public string Title { get; init; }

    public int Year { get; init; }

    public static int TotalItems => _totalItems;

    public virtual string Description => $"{Title}, {Year}";

    public virtual string GetInfo() => $"{Title} ({Year})";

    public abstract string GetCardInfo();
}

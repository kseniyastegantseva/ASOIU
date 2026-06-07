internal class Book : LibraryItem
{
    public Book(string title, int year, string author, int pageCount)
        : base(title, year)
    {
        Author = author;
        PageCount = pageCount;
        Console.WriteLine($"Вызван конструктор Book: {Title}");
    }

    public string Author { get; init; }

    public int PageCount { get; init; }

    public override string Description => $"Книга: {Title}, {Author}, {Year}";

    public override string GetInfo() => $"{Title} — {Author}, {Year}, {PageCount} стр.";

    public override string GetCardInfo() => $"[BOOK] {Title.ToUpperInvariant()} / {Author} / {Year}";
}

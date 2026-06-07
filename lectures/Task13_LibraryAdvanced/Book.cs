internal class Book : LibraryItem
{
    public Book(string title, string author, int year, BookGenre genre, ItemStatus status)
        : base(title, year, status)
    {
        Author = string.IsNullOrWhiteSpace(author)
            ? throw new InvalidBookDataException(nameof(Author), "Автор не может быть пустым.")
            : author;
        Genre = genre;
    }

    public string Author { get; }

    public BookGenre Genre { get; }

    public override string GetInfo() => $"Книга: {Title} — {Author}, {Year}, {Genre}, статус: {Status}";

    public override bool ContainsKeyword(string keyword, SearchOptions options)
    {
        return (options.HasFlag(SearchOptions.Title) && Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            || (options.HasFlag(SearchOptions.Author) && Author.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            || (options.HasFlag(SearchOptions.Genre) && Genre.ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    public override string ToCsvLine() => $"Book;{Title};{Author};{Year};{Genre};{Status}";
}

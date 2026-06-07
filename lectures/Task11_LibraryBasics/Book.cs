using System.Diagnostics.CodeAnalysis;

internal class Book
{
    public const int MaxPageCount = 10000;
    public const int MinYear = 1450;

    private static int _totalCount;
    private int _year;
    private int _pageCount;

    [SetsRequiredMembers]
    public Book()
    {
        Title = "Без названия";
        Author = "Неизвестный автор";
        Year = DateTime.Now.Year;
        PageCount = 1;
        Genre = "Не указан";
        _totalCount++;
    }

    [SetsRequiredMembers]
    public Book(string title, string author, int year, int pageCount, string genre)
    {
        Title = title;
        Author = author;
        Year = year;
        PageCount = pageCount;
        Genre = genre;
        _totalCount++;
    }

    public string Title { get; init; }

    public string Author { get; init; }

    public required string Genre { get; init; }

    public int Year
    {
        get => _year;
        set
        {
            if (value < MinYear || value > DateTime.Now.Year)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Год должен быть в диапазоне {MinYear}..{DateTime.Now.Year}.");
            }

            _year = value;
        }
    }

    public int PageCount
    {
        get => _pageCount;
        set
        {
            if (value <= 0 || value > MaxPageCount)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Количество страниц должно быть от 1 до {MaxPageCount}.");
            }

            _pageCount = value;
        }
    }

    public int AgeInYears => DateTime.Now.Year - Year;

    public string ShortDescription => $"{Title} — {Author}, {Year}";

    public static int TotalCount => _totalCount;

    public string GetInfo() => GetInfo(showPages: true);

    public string GetInfo(bool showPages)
    {
        string pages = showPages ? $", страниц: {PageCount}" : "";
        return $"{Title} — {Author}, {Year}, жанр: {Genre}{pages}";
    }

    public bool IsOlderThan(int years = 50) => AgeInYears > years;

    public string GetFormattedInfo(string format = "short")
    {
        string FormatFull() => $"{Title} | {Author} | {Year} | {PageCount} стр. | {Genre}";
        string FormatShort() => ShortDescription;

        return format.ToLowerInvariant() switch
        {
            "full" => FormatFull(),
            "short" => FormatShort(),
            _ => GetInfo()
        };
    }

    public static void PrintStatistics()
    {
        Console.WriteLine($"Всего создано книг: {TotalCount}");
    }
}

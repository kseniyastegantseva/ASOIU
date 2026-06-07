internal abstract class LibraryItem : ISearchable, IExportable
{
    private int _year;

    protected LibraryItem(string title, int year, ItemStatus status)
    {
        Title = string.IsNullOrWhiteSpace(title)
            ? throw new LibraryException("Название не может быть пустым.")
            : title;
        Year = year;
        Status = status;
    }

    public string Title { get; }

    public int Year
    {
        get => _year;
        init
        {
            if (value < 1450 || value > DateTime.Now.Year)
            {
                throw new InvalidBookDataException(nameof(Year), $"Год должен быть в диапазоне 1450..{DateTime.Now.Year}.");
            }

            _year = value;
        }
    }

    public ItemStatus Status { get; set; }

    public abstract string GetInfo();

    public abstract bool ContainsKeyword(string keyword, SearchOptions options);

    public abstract string ToCsvLine();
}

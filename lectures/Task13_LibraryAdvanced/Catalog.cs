using System.Text.Json;

internal class Catalog<T>
    where T : LibraryItem
{
    private readonly List<T> _items = [];

    public Catalog(string name)
    {
        Name = string.IsNullOrWhiteSpace(name)
            ? throw new LibraryException("Название каталога не может быть пустым.")
            : name;
    }

    public string Name { get; }

    public T this[int index] => index >= 0 && index < _items.Count
        ? _items[index]
        : throw new LibraryException($"Элемент с индексом {index} не найден.");

    public void Add(T item)
    {
        _items.Add(item ?? throw new LibraryException("Нельзя добавить пустой элемент."));
    }

    public IEnumerable<T> Search(string keyword, SearchOptions options)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            throw new LibraryException("Ключевое слово для поиска не задано.");
        }

        return _items.Where(item => item.ContainsKeyword(keyword, options));
    }

    public IEnumerable<T> FindOlderThan(int years)
    {
        int boundaryYear = DateTime.Now.Year - years;
        return _items.Where(item => item.Year < boundaryYear);
    }

    public void PrintAll()
    {
        foreach (T item in _items)
        {
            Console.WriteLine($"  {item.GetInfo()}");
        }
    }

    public string ExportCsv()
    {
        return "Type;Title;Extra;Year;Genre;Status"
            + Environment.NewLine
            + string.Join(Environment.NewLine, _items.Select(item => item.ToCsvLine()));
    }

    public string ExportJson()
    {
        var rows = _items.Select(item => new
        {
            Type = item.GetType().Name,
            item.Title,
            item.Year,
            item.Status,
            Info = item.GetInfo()
        });

        return JsonSerializer.Serialize(rows, new JsonSerializerOptions { WriteIndented = true });
    }

    public void AddFromConsole()
    {
        try
        {
            Console.Write("Название книги: ");
            string? title = Console.ReadLine();

            if (title is null)
            {
                Console.WriteLine("Ввод не получен, интерактивное добавление пропущено.");
                return;
            }

            Console.Write("Автор: ");
            string author = Console.ReadLine() ?? "";

            Console.Write("Год: ");
            string yearText = Console.ReadLine() ?? "";

            if (!int.TryParse(yearText, out int year))
            {
                throw new InvalidBookDataException(nameof(Book.Year), "Год должен быть целым числом.");
            }

            Book book = new(title, author, year, BookGenre.Unknown, ItemStatus.Available);

            if (book is T typedBook)
            {
                Add(typedBook);
                Console.WriteLine("Книга добавлена.");
            }
            else
            {
                throw new LibraryException("Тип каталога не позволяет добавить книгу.");
            }
        }
        catch (InvalidBookDataException ex) when (ex.FieldName == nameof(Book.Year))
        {
            Console.WriteLine($"Ошибка ввода года: {ex.Message}");
        }
        catch (LibraryException ex)
        {
            Console.WriteLine($"Ошибка добавления: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Попытка интерактивного добавления завершена.");
        }
    }
}

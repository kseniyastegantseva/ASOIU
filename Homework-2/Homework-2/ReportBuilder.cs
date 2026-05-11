using System.Text;

/// <summary>
/// Класс для построения текстовых отчётов через Fluent Interface.
/// </summary>
class ReportBuilder
{
    private readonly DatabaseManager _db;
    private string _sql = "";
    private string _title = "";
    private string[] _headers = Array.Empty<string>();
    private int[] _widths = Array.Empty<int>();

    /// <summary>
    /// Создаёт построитель отчётов с доступом к DatabaseManager.
    /// </summary>
    public ReportBuilder(DatabaseManager db)
    {
        _db = db;
    }

    /// <summary>
    /// Устанавливает SQL-запрос отчёта.
    /// </summary>
    public ReportBuilder Query(string sql)
    {
        _sql = sql;
        return this;
    }

    /// <summary>
    /// Устанавливает заголовок отчёта.
    /// </summary>
    public ReportBuilder Title(string text)
    {
        _title = text;
        return this;
    }

    /// <summary>
    /// Устанавливает названия колонок.
    /// </summary>
    public ReportBuilder Header(params string[] columns)
    {
        _headers = columns;
        return this;
    }

    /// <summary>
    /// Устанавливает ширины колонок.
    /// </summary>
    public ReportBuilder ColumnWidths(params int[] widths)
    {
        _widths = widths;
        return this;
    }

    /// <summary>
    /// Выполняет запрос и возвращает готовый текст отчёта.
    /// </summary>
    public string Build()
    {
        CsvTable table = _db.ExecuteQuery(_sql);
        var builder = new StringBuilder();

        if (_title.Length > 0)
        {
            builder.AppendLine($"=== {_title} ===");
            builder.AppendLine();
        }

        string[] displayHeaders = table.Headers;
        if (_headers.Length > 0)
            displayHeaders = _headers;

        int columnCount = displayHeaders.Length;
        int[] widths = BuildWidths(columnCount);

        for (int i = 0; i < columnCount; i++)
            AppendCell(builder, displayHeaders[i], widths[i]);
        builder.AppendLine();

        int totalWidth = 0;
        for (int i = 0; i < columnCount; i++)
            totalWidth += widths[i] + 2;
        builder.AppendLine(new string('-', totalWidth));

        for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
        {
            string[] fields = table.Rows[rowIndex].Fields;
            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                string value = "";
                if (columnIndex < fields.Length)
                    value = fields[columnIndex];

                AppendCell(builder, value, widths[columnIndex]);
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }

    /// <summary>
    /// Выводит отчёт в консоль.
    /// </summary>
    public void Print()
    {
        Console.Write(Build());
    }

    /// <summary>
    /// Сохраняет отчёт в текстовый файл.
    /// </summary>
    public void SaveToFile(string path)
    {
        string? directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllText(path, Build(), Encoding.UTF8);
        Console.WriteLine($"Отчёт сохранён в файл: {path}");
    }

    private int[] BuildWidths(int columnCount)
    {
        var widths = new int[columnCount];
        for (int i = 0; i < columnCount; i++)
        {
            if (i < _widths.Length)
                widths[i] = _widths[i];
            else
                widths[i] = 20;
        }

        return widths;
    }

    private string Cut(string value, int width)
    {
        if (value.Length <= width)
            return value;

        if (width <= 3)
            return value.Substring(0, width);

        return value.Substring(0, width - 3) + "...";
    }

    private void AppendCell(StringBuilder builder, string value, int width)
    {
        builder.Append(Cut(value, width).PadRight(width));
        builder.Append("  ");
    }
}

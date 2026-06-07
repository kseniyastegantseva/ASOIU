namespace ASOIU_3.UI;

// Вспомогательный класс отображения не знает о ресторанах и блюдах:
// он работает с обобщёнными интерфейсами коллекций и пригоден для любой таблицы строк.
internal static class ConsoleTable
{
    private const int MaximumColumnWidth = 40;

    public static void Print(
        // IReadOnlyList<string> фиксирует контракт заголовков, а IEnumerable<T>
        // позволяет принять в том числе отложенный результат LINQ Select.
        IReadOnlyList<string> headers,
        IEnumerable<IReadOnlyList<string>> rows)
    {
        // ToList материализует строки один раз, поскольку ниже коллекция обходится многократно.
        var materializedRows = rows.ToList();
        var widths = headers
            // Перегрузка Select передаёт в лямбду и значение, и индекс столбца.
            .Select((header, index) =>
                materializedRows
                    .Select(row => row[index])
                    .Append(header)
                    .Max(value => Math.Min(value.Length, MaximumColumnWidth)))
            .ToArray();

        PrintRow(headers, widths);
        Console.WriteLine(string.Join("-+-", widths.Select(width => new string('-', width))));

        foreach (var row in materializedRows)
        {
            // foreach отвечает только за последовательный вывод уже подготовленной коллекции.
            PrintRow(row, widths);
        }
    }

    private static void PrintRow(IReadOnlyList<string> values, IReadOnlyList<int> widths)
    {
        var cells = values.Select(
            // index используется для сопоставления значения с шириной того же столбца.
            (value, index) => Truncate(value, widths[index]).PadRight(widths[index]));
        Console.WriteLine(string.Join(" | ", cells));
    }

    private static string Truncate(string value, int width)
    {
        if (value.Length <= width)
        {
            return value;
        }

        return width <= 3 ? value[..width] : $"{value[..(width - 3)]}...";
    }
}

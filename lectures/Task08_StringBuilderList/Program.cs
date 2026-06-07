using System.Text;

List<string> items = [];

Console.WriteLine("Формирование нумерованного списка через StringBuilder");
Console.WriteLine("Введите строки. Пустая строка завершает ввод.");

while (true)
{
    Console.Write("> ");
    string? line = Console.ReadLine();

    if (line is null)
    {
        Console.WriteLine();
        Console.WriteLine("Ввод не получен, используются демонстрационные строки.");
        items.AddRange(["Подготовить конспект", "Проверить примеры", "Собрать проект"]);
        break;
    }

    if (string.IsNullOrWhiteSpace(line))
    {
        break;
    }

    items.Add(line);
}

if (items.Count == 0)
{
    items.AddRange(["Первая строка", "Вторая строка", "Третья строка"]);
}

string listFile = Path.Combine(AppContext.BaseDirectory, "list.txt");
string content = BuildNumberedList(items);
File.WriteAllText(listFile, content);

Console.WriteLine();
Console.WriteLine($"Список сохранен в файл: {listFile}");
Console.WriteLine();
Console.WriteLine("Содержимое list.txt:");

foreach (string fileLine in File.ReadLines(listFile))
{
    Console.WriteLine(fileLine);
}

static string BuildNumberedList(IReadOnlyList<string> items)
{
    StringBuilder builder = new();
    string date = DateTime.Now.ToString("dd.MM.yyyy");

    for (int i = 0; i < items.Count; i++)
    {
        builder.AppendLine($"{i + 1}. [{date}] {items[i]}");
    }

    return builder.ToString();
}

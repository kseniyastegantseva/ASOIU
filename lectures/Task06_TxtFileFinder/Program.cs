Console.WriteLine("================================");
Console.WriteLine("   ПОИСК ТЕКСТОВЫХ ФАЙЛОВ (.txt)");
Console.WriteLine("================================");
Console.WriteLine();

Console.Write("Введите путь к каталогу (Enter для текущего каталога): ");
string inputPath = Console.ReadLine()?.Trim() ?? "";
string directoryPath = string.IsNullOrWhiteSpace(inputPath)
    ? Directory.GetCurrentDirectory()
    : inputPath;

Console.WriteLine();

if (!Directory.Exists(directoryPath))
{
    Console.Error.WriteLine($"Ошибка: каталог не существует: {directoryPath}");
    return;
}

SearchTextFiles(directoryPath);

static void SearchTextFiles(string rootPath)
{
    Console.WriteLine($"Поиск в каталоге: {rootPath}");
    Console.WriteLine();

    string[] files = Directory.GetFiles(rootPath, "*.txt", SearchOption.AllDirectories);

    if (files.Length == 0)
    {
        Console.WriteLine("Текстовые файлы (.txt) не найдены.");
        return;
    }

    PrintTableHeader();

    foreach (string filePath in files)
    {
        PrintFileInfo(filePath);
    }

    Console.WriteLine(new string('-', 120));
    Console.WriteLine($"Итого найдено файлов: {files.Length}");
}

static void PrintTableHeader()
{
    Console.WriteLine(new string('-', 120));
    Console.WriteLine($"{"Имя файла",-28} {"Дата изменения",-18} {"Каталог",-35} Полный путь");
    Console.WriteLine(new string('-', 120));
}

static void PrintFileInfo(string filePath)
{
    string fileName = Path.GetFileName(filePath);
    string fullPath = Path.GetFullPath(filePath);
    string directory = Path.GetDirectoryName(fullPath) ?? "";
    DateTime lastModified = File.GetLastWriteTime(fullPath);

    Console.WriteLine($"{fileName,-28} {lastModified:dd.MM.yyyy HH:mm}   {Shorten(directory, 35),-35} {fullPath}");
}

static string Shorten(string value, int maxLength)
{
    if (value.Length <= maxLength)
    {
        return value;
    }

    return "..." + value[^Math.Max(0, maxLength - 3)..];
}

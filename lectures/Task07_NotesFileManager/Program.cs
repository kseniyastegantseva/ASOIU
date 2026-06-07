string notesFile = Path.Combine(AppContext.BaseDirectory, "notes.txt");

if (!File.Exists(notesFile))
{
    Console.WriteLine("Файл заметок не найден, будет создан новый.");
    File.WriteAllText(notesFile, "");
}
else
{
    Console.WriteLine($"Файл заметок найден: {notesFile}");
}

Console.WriteLine();

while (true)
{
    PrintMenu();
    string? choice = Console.ReadLine();

    if (choice is null)
    {
        Console.WriteLine();
        Console.WriteLine("Ввод не получен. Демонстрация текущих заметок:");
        ShowAllNotes(notesFile);
        break;
    }

    Console.WriteLine();

    switch (choice.Trim())
    {
        case "1":
            AddNote(notesFile);
            break;
        case "2":
            ShowAllNotes(notesFile);
            break;
        case "3":
            SearchNotes(notesFile);
            break;
        case "4":
            ClearNotes(notesFile);
            break;
        case "0":
            Console.WriteLine("До свидания!");
            return;
        default:
            Console.WriteLine("Неверный пункт меню. Попробуйте снова.");
            break;
    }

    Console.WriteLine();
}

static void PrintMenu()
{
    Console.WriteLine("================================");
    Console.WriteLine("        МЕНЕДЖЕР ЗАМЕТОК       ");
    Console.WriteLine("================================");
    Console.WriteLine("  1 — Добавить заметку");
    Console.WriteLine("  2 — Показать все заметки");
    Console.WriteLine("  3 — Найти заметки по слову");
    Console.WriteLine("  4 — Очистить все заметки");
    Console.WriteLine("  0 — Выход");
    Console.WriteLine("================================");
    Console.Write("Ваш выбор: ");
}

static void AddNote(string filePath)
{
    Console.Write("Введите текст заметки: ");
    string noteText = Console.ReadLine() ?? "";

    if (string.IsNullOrWhiteSpace(noteText))
    {
        Console.WriteLine("Заметка не добавлена: текст пустой.");
        return;
    }

    string timestamp = DateTime.Now.ToString("[dd.MM.yyyy HH:mm]");
    File.AppendAllText(filePath, $"{timestamp} {noteText}{Environment.NewLine}");
    Console.WriteLine("Заметка успешно добавлена.");
}

static void ShowAllNotes(string filePath)
{
    Console.WriteLine("-------- Все заметки ----------");

    int lineNumber = 1;

    foreach (string line in File.ReadLines(filePath))
    {
        if (!string.IsNullOrWhiteSpace(line))
        {
            Console.WriteLine($"{lineNumber,3}. {line}");
            lineNumber++;
        }
    }

    if (lineNumber == 1)
    {
        Console.WriteLine("Заметок пока нет.");
    }
    else
    {
        Console.WriteLine("-------------------------------");
        Console.WriteLine($"Итого заметок: {lineNumber - 1}");
    }
}

static void SearchNotes(string filePath)
{
    Console.Write("Введите слово для поиска: ");
    string searchWord = Console.ReadLine() ?? "";

    if (string.IsNullOrWhiteSpace(searchWord))
    {
        Console.WriteLine("Слово для поиска не введено.");
        return;
    }

    Console.WriteLine($"---- Поиск по слову \"{searchWord}\" ----");

    int foundCount = 0;
    int lineNumber = 1;

    foreach (string line in File.ReadLines(filePath))
    {
        if (!string.IsNullOrWhiteSpace(line))
        {
            if (line.Contains(searchWord, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"{lineNumber,3}. {line}");
                foundCount++;
            }

            lineNumber++;
        }
    }

    Console.WriteLine("-------------------------------");
    Console.WriteLine(foundCount == 0
        ? $"Заметок со словом \"{searchWord}\" не найдено."
        : $"Найдено заметок: {foundCount}");
}

static void ClearNotes(string filePath)
{
    Console.Write("Вы уверены, что хотите удалить все заметки? (да/нет): ");
    string confirm = Console.ReadLine()?.Trim().ToLowerInvariant() ?? "";

    if (confirm == "да")
    {
        File.WriteAllText(filePath, "");
        Console.WriteLine("Все заметки удалены.");
    }
    else
    {
        Console.WriteLine("Очистка отменена.");
    }
}

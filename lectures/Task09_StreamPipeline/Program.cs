string[] surnames =
[
    "Иванов", "Петров", "Сидоров", "Кузнецов", "Смирнов",
    "Попов", "Волков", "Новиков"
];

string[] subjects =
[
    "Математика", "Физика", "Информатика", "История", "Химия"
];

if (args.Length == 0)
{
    PrintUsage();
    return;
}

string mode = args[0].ToLowerInvariant();

switch (mode)
{
    case "generate":
        Generate();
        break;

    case "filter":
        if (args.Length < 2)
        {
            Console.Error.WriteLine("[ОШИБКА] Режим filter требует второй аргумент — минимальную оценку.");
            Console.Error.WriteLine("Пример: dotnet run --project Task09_StreamPipeline -- filter 4");
            return;
        }

        if (!int.TryParse(args[1], out int minGrade) || minGrade is < 2 or > 5)
        {
            Console.Error.WriteLine($"[ОШИБКА] Некорректная оценка: \"{args[1]}\". Допустимые значения: 2..5.");
            return;
        }

        Filter(minGrade);
        break;

    default:
        Console.Error.WriteLine($"[ОШИБКА] Неизвестный режим: \"{mode}\".");
        PrintUsage();
        break;
}

void PrintUsage()
{
    Console.Error.WriteLine("=== Программа обработки оценок ===");
    Console.Error.WriteLine("Использование:");
    Console.Error.WriteLine("  dotnet run --project Task09_StreamPipeline -- generate");
    Console.Error.WriteLine("  dotnet run --project Task09_StreamPipeline -- filter <мин_оценка>");
    Console.Error.WriteLine();
    Console.Error.WriteLine("Пример pipeline:");
    Console.Error.WriteLine("  dotnet run --project Task09_StreamPipeline -- generate | dotnet run --project Task09_StreamPipeline -- filter 4");
}

void Generate()
{
    Console.Error.WriteLine("[generate] Генерация 20 записей...");
    Random random = new(42);

    for (int i = 0; i < 20; i++)
    {
        string surname = surnames[random.Next(surnames.Length)];
        string subject = subjects[random.Next(subjects.Length)];
        int grade = random.Next(2, 6);

        Console.WriteLine($"{surname};{subject};{grade}");
    }

    Console.Error.WriteLine("[generate] Готово.");
}

void Filter(int minGrade)
{
    Console.Error.WriteLine($"[filter] Минимальная оценка: {minGrade}");

    string? line;
    int total = 0;
    int passed = 0;

    while ((line = Console.ReadLine()) is not null)
    {
        total++;

        string[] parts = line.Split(';');
        if (parts.Length != 3 || !int.TryParse(parts[2], out int grade))
        {
            Console.Error.WriteLine($"[filter] Пропущена некорректная строка: {line}");
            continue;
        }

        if (grade >= minGrade)
        {
            Console.WriteLine(line);
            passed++;
        }
    }

    Console.Error.WriteLine($"[filter] Обработано строк: {total}, прошло фильтр: {passed}");
}

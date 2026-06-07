Console.WriteLine("Упрощенная система учета студентов на стандартных коллекциях");

ShowStudentsList();
ShowExamHistoryStack();
ShowLabQueue();
ShowGradesDictionary();

static void ShowStudentsList()
{
    PrintSection("1. Список студентов (List<string>)");

    List<string> students =
    [
        "Иванов",
        "Петров",
        "Сидоров",
        "Козлов",
        "Новиков"
    ];

    Console.WriteLine("Исходный список:");
    PrintList(students);

    students.Add("Морозов");
    Console.WriteLine();
    Console.WriteLine("После добавления Морозова:");
    PrintList(students);

    string searchName = "Петров";
    Console.WriteLine();
    Console.WriteLine($"Студент '{searchName}' в списке: {students.Contains(searchName)}");

    students.Remove("Сидоров");
    Console.WriteLine();
    Console.WriteLine("После удаления Сидорова:");
    PrintNumberedList(students);
}

static void ShowExamHistoryStack()
{
    PrintSection("2. История ответов на экзамене (Stack<string>)");

    Stack<string> examHistory = new();
    examHistory.Push("Иванов");
    examHistory.Push("Козлов");
    examHistory.Push("Новиков");

    Console.WriteLine($"Последний ответивший (Peek): {examHistory.Peek()}");
    Console.WriteLine("Извлечение из стека (порядок LIFO):");

    while (examHistory.Count > 0)
    {
        Console.WriteLine($"  Ответил: {examHistory.Pop()}");
    }
}

static void ShowLabQueue()
{
    PrintSection("3. Очередь на сдачу лабораторной (Queue<string>)");

    Queue<string> labQueue = new();
    labQueue.Enqueue("Иванов");
    labQueue.Enqueue("Петров");
    labQueue.Enqueue("Козлов");
    labQueue.Enqueue("Морозов");

    Console.WriteLine($"Первый в очереди (Peek): {labQueue.Peek()}");
    Console.WriteLine("Прием лабораторных (порядок FIFO):");

    while (labQueue.Count > 0)
    {
        Console.WriteLine($"  Принята работа у: {labQueue.Dequeue()}");
    }
}

static void ShowGradesDictionary()
{
    PrintSection("4. Оценки студентов (Dictionary<string, int>)");

    Dictionary<string, int> grades = new()
    {
        ["Иванов"] = 5,
        ["Петров"] = 4,
        ["Козлов"] = 3,
        ["Новиков"] = 5,
        ["Морозов"] = 4
    };

    Console.WriteLine("Все оценки:");
    foreach (var (name, grade) in grades)
    {
        Console.WriteLine($"  {name}: {grade}");
    }

    string target = "Козлов";
    if (grades.TryGetValue(target, out int foundGrade))
    {
        Console.WriteLine();
        Console.WriteLine($"Оценка {target}: {foundGrade}");
    }

    double average = 0;
    foreach (var (_, grade) in grades)
    {
        average += grade;
    }

    average /= grades.Count;
    Console.WriteLine($"Средняя оценка по группе: {average:F2}");
}

static void PrintSection(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 46));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 46));
}

static void PrintList(IEnumerable<string> values)
{
    foreach (string value in values)
    {
        Console.WriteLine($"  {value}");
    }
}

static void PrintNumberedList(IReadOnlyList<string> values)
{
    for (int i = 0; i < values.Count; i++)
    {
        Console.WriteLine($"  {i + 1}. {values[i]}");
    }
}

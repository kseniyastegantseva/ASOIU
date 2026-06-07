object?[] data = [15, "Hello", -5, null, "World", 42, 0, "C#", -10];

Console.WriteLine("Анализ массива object[] с pattern matching");
Console.WriteLine();

Console.WriteLine("=== Анализ с помощью if + is ===");
AnalyzeWithIf(data);

Console.WriteLine();
Console.WriteLine("=== Анализ с помощью switch ===");
AnalyzeWithSwitch(data);

Console.WriteLine();
Console.WriteLine("=== Категории через switch expression ===");
foreach (object? item in data)
{
    Console.WriteLine($"{FormatValue(item),-8} -> {GetCategory(item)}");
}

static void AnalyzeWithIf(object?[] array)
{
    foreach (object? obj in array)
    {
        if (obj is null)
        {
            Console.WriteLine("Пустое значение");
        }
        else if (obj is int positiveNumber && positiveNumber > 0)
        {
            Console.WriteLine($"Положительное число: {positiveNumber}");
        }
        else if (obj is int zeroNumber && zeroNumber == 0)
        {
            Console.WriteLine($"Ноль: {zeroNumber}");
        }
        else if (obj is int negativeNumber)
        {
            Console.WriteLine($"Отрицательное число: {negativeNumber}");
        }
        else if (obj is string text)
        {
            Console.WriteLine($"Строка: {text}");
        }
    }
}

static void AnalyzeWithSwitch(object?[] array)
{
    foreach (object? obj in array)
    {
        switch (obj)
        {
            case null:
                Console.WriteLine("Пустое значение");
                break;
            case int number when number > 0:
                Console.WriteLine($"Положительное число: {number}");
                break;
            case int number when number == 0:
                Console.WriteLine($"Ноль: {number}");
                break;
            case int number:
                Console.WriteLine($"Отрицательное число: {number}");
                break;
            case string text:
                Console.WriteLine($"Строка: {text}");
                break;
        }
    }
}

static string GetCategory(object? item) => item switch
{
    null => "Пустое значение",
    int number when number > 0 => $"Положительное число: {number}",
    int number when number == 0 => $"Ноль: {number}",
    int number => $"Отрицательное число: {number}",
    string text => $"Строка: {text}",
    _ => $"Неизвестный тип: {item.GetType().Name}"
};

static string FormatValue(object? value) => value?.ToString() ?? "null";

int?[] numbers = [10, null, 25, null, 30, 15, null];

Console.WriteLine("Демонстрация работы с null и nullable-значениями");
Console.WriteLine();

Console.WriteLine("=== 1. Исходный массив ===");
PrintNullableArray(numbers);

Console.WriteLine();
Console.WriteLine("=== 2. Подсчет null через is null ===");
int nullCount = CountNulls(numbers);
Console.WriteLine($"Количество null-значений: {nullCount}");

Console.WriteLine();
Console.WriteLine("=== 3. Подсчет не-null через is not null ===");
int notNullCount = CountNotNulls(numbers);
Console.WriteLine($"Количество не-null значений: {notNullCount}");

Console.WriteLine();
Console.WriteLine("=== 4. Сумма ненулевых чисел через HasValue и Value ===");
int sum = SumExistingValues(numbers);
Console.WriteLine($"Итоговая сумма: {sum}");

Console.WriteLine();
Console.WriteLine("=== 5. Новый массив: null заменены на -1 через ?? ===");
int[] numbersWithDefault = ReplaceNullsWithDefault(numbers, -1);
Console.WriteLine(string.Join(", ", numbersWithDefault));

Console.WriteLine();
Console.WriteLine("=== 6. Длина строкового представления через ?. ===");
for (int i = 0; i < numbers.Length; i++)
{
    int length = numbers[i]?.ToString()?.Length ?? 0;
    Console.WriteLine($"numbers[{i}] -> длина {length}");
}

Console.WriteLine();
Console.WriteLine("=== 7. Замена null на 0 в исходном массиве через ??= ===");
for (int i = 0; i < numbers.Length; i++)
{
    numbers[i] ??= 0;
}

Console.WriteLine();
Console.WriteLine("=== 8. Итоговый массив ===");
PrintNullableArray(numbers);

static void PrintNullableArray(int?[] array)
{
    for (int i = 0; i < array.Length; i++)
    {
        string display = array[i]?.ToString() ?? "null";
        Console.WriteLine($"numbers[{i}] = {display}");
    }
}

static int CountNulls(int?[] array)
{
    int count = 0;

    foreach (int? number in array)
    {
        if (number is null)
        {
            count++;
        }
    }

    return count;
}

static int CountNotNulls(int?[] array)
{
    int count = 0;

    foreach (int? number in array)
    {
        if (number is not null)
        {
            count++;
        }
    }

    return count;
}

static int SumExistingValues(int?[] array)
{
    int sum = 0;

    for (int i = 0; i < array.Length; i++)
    {
        int? value = array[i];

        if (value.HasValue)
        {
            sum += value.Value;
            Console.WriteLine($"Добавляем numbers[{i}] = {value.Value}, текущая сумма = {sum}");
        }
    }

    return sum;
}

static int[] ReplaceNullsWithDefault(int?[] array, int defaultValue)
{
    int[] result = new int[array.Length];

    for (int i = 0; i < array.Length; i++)
    {
        result[i] = array[i] ?? defaultValue;
    }

    return result;
}

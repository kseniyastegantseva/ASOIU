int[] numbers = [5, -3, 8, 12, -7, 0, 15, -2, 4, 20];

Console.WriteLine("Операции с массивом через разные циклы");
Console.WriteLine($"Исходный массив: {string.Join(", ", numbers)}");
Console.WriteLine();

Console.WriteLine("=== 1. Элементы на четных индексах (for) ===");
PrintEvenIndices(numbers);

Console.WriteLine();
Console.WriteLine("=== 2. Сумма всех элементов (foreach) ===");
Console.WriteLine($"Сумма: {CalculateSum(numbers)}");

Console.WriteLine();
Console.WriteLine("=== 3. Первое число больше 10 (while + break) ===");
int found = FindFirstGreaterThan(numbers, 10);
Console.WriteLine(found == -1 ? "Не найдено" : $"Найдено: {found}");

Console.WriteLine();
Console.WriteLine("=== 4. Вывод до превышения суммы 15 (do-while) ===");
PrintUntilSumExceeds(numbers, 15);

Console.WriteLine();
Console.WriteLine("=== 5. Только положительные числа (foreach + continue) ===");
PrintPositiveOnly(numbers);

static void PrintEvenIndices(int[] array)
{
    Console.Write("Элементы на индексах 0, 2, 4, ...: ");

    for (int i = 0; i < array.Length; i += 2)
    {
        Console.Write($"{array[i]} ");
    }

    Console.WriteLine();
}

static int CalculateSum(int[] array)
{
    int sum = 0;

    foreach (int number in array)
    {
        sum += number;
    }

    return sum;
}

static int FindFirstGreaterThan(int[] array, int threshold)
{
    int index = 0;
    int result = -1;

    while (index < array.Length)
    {
        if (array[index] > threshold)
        {
            result = array[index];
            break;
        }

        index++;
    }

    return result;
}

static void PrintUntilSumExceeds(int[] array, int limit)
{
    int index = 0;
    int currentSum = 0;

    do
    {
        currentSum += array[index];
        Console.WriteLine($"array[{index}] = {array[index]}, накопленная сумма = {currentSum}");
        index++;

        if (currentSum > limit)
        {
            Console.WriteLine($"Сумма превысила {limit}, выход из цикла");
            break;
        }
    }
    while (index < array.Length);
}

static void PrintPositiveOnly(int[] array)
{
    Console.Write("Положительные числа: ");

    foreach (int number in array)
    {
        if (number <= 0)
        {
            continue;
        }

        Console.Write($"{number} ");
    }

    Console.WriteLine();
}

Console.WriteLine("Решение биквадратного уравнения: ax^4 + bx^2 + c = 0");
Console.Write("Введите коэффициенты a, b, c через пробел (Enter для примера 1 -5 4): ");

string? input = Console.ReadLine();
double[] coefficients = string.IsNullOrWhiteSpace(input)
    ? [1, -5, 4]
    : input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(double.Parse)
        .ToArray();

double a = coefficients[0];
double b = coefficients[1];
double c = coefficients[2];

double[] roots = new double[4];
int count = SolveBiquadratic(a, b, c, roots);

Console.WriteLine();
Console.WriteLine($"Уравнение: {a:F2}x^4 + {b:F2}x^2 + {c:F2} = 0");
Console.WriteLine();
Console.WriteLine("Корни до сортировки:");
PrintArray(roots, count);

double[] sortedRoots = CopyArray(roots, count);
BubbleSort(sortedRoots, count);

Console.WriteLine();
Console.WriteLine("Корни после сортировки по возрастанию:");
PrintArray(sortedRoots, count);

Console.WriteLine();
Console.WriteLine("Тестовые примеры из методички:");
Console.WriteLine("  a=1, b=-5,  c=4  -> корни: -2, -1, 1, 2");
Console.WriteLine("  a=1, b=-10, c=9  -> корни: -3, -1, 1, 3");
Console.WriteLine("  a=1, b=-13, c=36 -> корни: -3, -2, 2, 3");

static int SolveBiquadratic(double a, double b, double c, double[] roots)
{
    double discriminant = b * b - 4 * a * c;
    double y1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
    double y2 = (-b - Math.Sqrt(discriminant)) / (2 * a);

    roots[0] = Math.Sqrt(y1);
    roots[1] = -Math.Sqrt(y1);
    roots[2] = Math.Sqrt(y2);
    roots[3] = -Math.Sqrt(y2);

    return 4;
}

// Используется пузырьковая сортировка (Bubble Sort).
static void BubbleSort(double[] array, int length)
{
    for (int i = 0; i < length - 1; i++)
    {
        for (int j = 0; j < length - i - 1; j++)
        {
            if (array[j] > array[j + 1])
            {
                (array[j], array[j + 1]) = (array[j + 1], array[j]);
            }
        }
    }
}

static double[] CopyArray(double[] source, int length)
{
    double[] result = new double[length];

    for (int i = 0; i < length; i++)
    {
        result[i] = source[i];
    }

    return result;
}

static void PrintArray(double[] array, int length)
{
    for (int i = 0; i < length; i++)
    {
        Console.WriteLine($"  x{i + 1} = {array[i],7:F3}");
    }
}

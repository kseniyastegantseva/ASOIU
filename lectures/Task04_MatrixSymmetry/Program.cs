int[,] symmetric =
{
    { 1, 2, 3 },
    { 2, 5, 6 },
    { 3, 6, 9 }
};

int[,] notSymmetric =
{
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

int[,] notSquare =
{
    { 1, 2, 3 },
    { 4, 5, 6 }
};

CheckMatrix("Матрица 1: симметричная", symmetric);
CheckMatrix("Матрица 2: несимметричная", notSymmetric);
CheckMatrix("Матрица 3: неквадратная", notSquare);

static void CheckMatrix(string title, int[,] matrix)
{
    Console.WriteLine($"=== {title} ===");
    PrintMatrix(matrix);
    Console.WriteLine($"Симметрична: {(IsSymmetric(matrix) ? "Да" : "Нет")}");
    Console.WriteLine();
}

static bool IsSymmetric(int[,] matrix)
{
    int rows = matrix.GetLength(0);
    int columns = matrix.GetLength(1);

    if (rows != columns)
    {
        return false;
    }

    for (int row = 0; row < rows; row++)
    {
        for (int column = row + 1; column < columns; column++)
        {
            if (matrix[row, column] != matrix[column, row])
            {
                return false;
            }
        }
    }

    return true;
}

static void PrintMatrix(int[,] matrix)
{
    int rows = matrix.GetLength(0);
    int columns = matrix.GetLength(1);

    for (int row = 0; row < rows; row++)
    {
        for (int column = 0; column < columns; column++)
        {
            Console.Write($"{matrix[row, column],4}");
        }

        Console.WriteLine();
    }
}

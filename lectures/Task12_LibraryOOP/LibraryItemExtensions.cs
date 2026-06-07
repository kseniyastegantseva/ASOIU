internal static class LibraryItemExtensions
{
    public static bool IsNew(this LibraryItem item, int years = 5)
    {
        return DateTime.Now.Year - item.Year <= years;
    }

    public static string ToCsvLine(this LibraryItem item)
    {
        string extra = item switch
        {
            Book book => $"Автор: {book.Author};Страниц: {book.PageCount}",
            Magazine magazine => $"Выпуск: {magazine.IssueNumber}",
            _ => ""
        };

        return $"{item.GetType().Name};{item.Title};{item.Year};{extra}";
    }

    public static void PrintCard(this LibraryItem item)
    {
        Console.WriteLine(new string('-', 36));
        Console.WriteLine(item.GetCardInfo());
        Console.WriteLine(item.Description);
        Console.WriteLine(new string('-', 36));
    }
}

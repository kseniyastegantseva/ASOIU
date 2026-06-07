internal class Magazine : LibraryItem
{
    public Magazine(string title, int year, int issueNumber, ItemStatus status)
        : base(title, year, status)
    {
        IssueNumber = issueNumber > 0
            ? issueNumber
            : throw new LibraryException("Номер выпуска должен быть положительным.");
    }

    public int IssueNumber { get; }

    public override string GetInfo() => $"Журнал: {Title}, выпуск {IssueNumber}, {Year}, статус: {Status}";

    public override bool ContainsKeyword(string keyword, SearchOptions options)
    {
        return options.HasFlag(SearchOptions.Title)
            && Title.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }

    public override string ToCsvLine() => $"Magazine;{Title};Issue {IssueNumber};{Year};;{Status}";
}

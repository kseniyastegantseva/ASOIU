internal class Magazine : LibraryItem
{
    public Magazine(string title, int year, int issueNumber)
        : base(title, year)
    {
        IssueNumber = issueNumber;
        Console.WriteLine($"Вызван конструктор Magazine: {Title}");
    }

    public int IssueNumber { get; init; }

    public override string Description => $"Журнал: {Title}, выпуск {IssueNumber}, {Year}";

    public override string GetInfo() => $"{Title} — выпуск {IssueNumber}, {Year}";

    public override string GetCardInfo() => $"[MAG] {Title.ToUpperInvariant()} / N{IssueNumber} / {Year}";
}

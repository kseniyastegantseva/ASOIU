internal partial class Library
{
    private readonly List<LibraryItem> _items = [];

    public Library Add(LibraryItem item)
    {
        _items.Add(item);
        return this;
    }

    public void PrintAll()
    {
        Console.WriteLine("Все элементы библиотеки:");

        foreach (LibraryItem item in _items)
        {
            Console.WriteLine($"  {item.GetInfo()}");
        }
    }
}

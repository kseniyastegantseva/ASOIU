internal partial class Library
{
    public IEnumerable<LibraryItem> FindByTitle(string keyword)
    {
        return _items.Where(item => item.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Book> FindBooks()
    {
        return _items.OfType<Book>();
    }

    public IEnumerable<Magazine> FindMagazines()
    {
        return _items.OfType<Magazine>();
    }
}

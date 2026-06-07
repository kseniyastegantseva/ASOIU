internal interface ISearchable
{
    bool ContainsKeyword(string keyword, SearchOptions options);
}

internal interface IExportable
{
    string ToCsvLine();
}

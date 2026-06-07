internal class LibraryException : Exception
{
    public LibraryException(string message)
        : base(message)
    {
    }
}

internal class InvalidBookDataException : LibraryException
{
    public InvalidBookDataException(string fieldName, string message)
        : base(message)
    {
        FieldName = fieldName;
    }

    public string FieldName { get; }
}

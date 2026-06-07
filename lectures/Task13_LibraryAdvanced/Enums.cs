[Flags]
internal enum SearchOptions
{
    None = 0,
    Title = 1,
    Author = 2,
    Genre = 4,
    All = Title | Author | Genre
}

internal enum BookGenre
{
    Unknown,
    Novel,
    SciFi,
    Detective,
    History
}

internal enum ItemStatus
{
    Available,
    CheckedOut,
    Archived
}

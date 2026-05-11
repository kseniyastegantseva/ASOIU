/// <summary>
/// Ресторан из справочной таблицы restaurants.
/// </summary>
class Restaurant
{
    /// <summary>
    /// Идентификатор ресторана.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название ресторана.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Конструктор с параметрами.
    /// </summary>
    public Restaurant(int id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Restaurant() : this(0, "") { }

    /// <summary>
    /// Возвращает строковое представление ресторана.
    /// </summary>
    public override string ToString()
    {
        return $"[{Id}] {Name}";
    }
}

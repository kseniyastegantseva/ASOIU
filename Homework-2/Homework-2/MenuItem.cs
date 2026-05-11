/// <summary>
/// Блюдо в меню из основной таблицы menu_items.
/// </summary>
class MenuItem
{
    private int _price;

    /// <summary>
    /// Идентификатор блюда.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор ресторана, к которому относится блюдо.
    /// </summary>
    public int RestaurantId { get; set; }

    /// <summary>
    /// Название блюда.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Цена блюда в рублях. Не может быть отрицательной.
    /// </summary>
    public int Price
    {
        get
        {
            return _price;
        }
        set
        {
            if (value < 0)
                throw new ArgumentException("Цена блюда не может быть отрицательной.");

            _price = value;
        }
    }

    /// <summary>
    /// Конструктор с параметрами.
    /// </summary>
    public MenuItem(int id, int restaurantId, string name, int price)
    {
        Id = id;
        RestaurantId = restaurantId;
        Name = name;
        Price = price;
    }

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public MenuItem() : this(0, 0, "", 0) { }

    /// <summary>
    /// Возвращает строковое представление блюда.
    /// </summary>
    public override string ToString()
    {
        return $"[{Id}] {Name}, ресторан #{RestaurantId}, цена: {Price} руб.";
    }
}

namespace ASOIU_3.Models;

/// <summary>
/// Ресторан из справочной таблицы, представляющий сторону «один».
/// </summary>
// Entity-класс описывает объект предметной области, который EF Core хранит
// как отдельную запись в таблице Restaurants.
public sealed class Restaurant
{
    /// <summary>
    /// Получает или задаёт идентификатор ресторана.
    /// </summary>
    // По соглашениям EF Core свойство Id становится первичным ключом таблицы:
    // оно однозначно идентифицирует ресторан и связывает его с блюдами.
    public int Id { get; set; }

    /// <summary>
    /// Получает или задаёт название ресторана.
    /// </summary>
    // Name — обычное поле таблицы Restaurants с данными, которые вводит пользователь.
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задаёт коллекцию блюд ресторана.
    /// </summary>
    // ICollection<MenuItem> — обобщённая коллекция: тип MenuItem гарантирует,
    // что в ней могут находиться только блюда, а не произвольные объекты.
    // MenuItems — навигационное свойство EF Core для стороны «один» связи one-to-many:
    // один ресторан может иметь много связанных блюд.
    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}

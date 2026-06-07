namespace ASOIU_3.Models;

/// <summary>
/// Блюдо в меню из основной таблицы, представляющее сторону «много».
/// </summary>
// Entity-класс соответствует основной таблице MenuItems, над которой выполняется CRUD.
public sealed class MenuItem
{
    /// <summary>
    /// Получает или задаёт идентификатор блюда.
    /// </summary>
    // Id — первичный ключ таблицы MenuItems по соглашениям EF Core.
    public int Id { get; set; }

    /// <summary>
    /// Получает или задаёт идентификатор ресторана.
    /// </summary>
    // RestaurantId — внешний ключ: в каждой записи блюда он хранит Id ресторана-владельца.
    public int RestaurantId { get; set; }

    /// <summary>
    /// Получает или задаёт ресторан, которому принадлежит блюдо.
    /// </summary>
    // Restaurant — навигационное свойство на связанную Entity-модель ресторана.
    // Пара RestaurantId/Restaurant позволяет EF Core построить связь между таблицами.
    public Restaurant Restaurant { get; set; } = null!;

    /// <summary>
    /// Получает или задаёт название блюда.
    /// </summary>
    // Name — обычное текстовое поле основной таблицы.
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Получает или задаёт цену блюда в рублях.
    /// </summary>
    // Price — числовое поле варианта 22; оно проверяется на неотрицательность
    // и используется при расчёте средней цены в отчёте.
    public double Price { get; set; }
}

using ASOIU_3.Models;

namespace ASOIU_3.Data;

/// <summary>
/// Создаёт базу данных и добавляет демонстрационные данные.
/// </summary>
// Инициализатор отделяет подготовку базы от пользовательского интерфейса,
// чтобы Console и WinForms запускались с одинаковой схемой и начальными данными.
public static class DatabaseInitializer
{
    /// <summary>
    /// Создаёт базу данных при первом запуске и заполняет пустую базу.
    /// </summary>
    public static void Initialize()
    {
        using var context = new AppDbContext();
        // EnsureCreated создаёт файл базы и таблицы при первом запуске.
        // Если база уже существует, пользовательские данные не пересоздаются.
        context.Database.EnsureCreated();

        // Any — LINQ-метод, который проверяет наличие хотя бы одной записи.
        // Проверка не допускает повторного добавления демонстрационных данных.
        if (context.Restaurants.Any())
        {
            return;
        }

        var restaurants = CreateRestaurants();
        context.Restaurants.AddRange(restaurants);
        // SaveChanges формирует INSERT-команды и сохраняет весь граф ресторанов
        // вместе с их связанными блюдами в SQLite.
        context.SaveChanges();
    }

    private static Restaurant[] CreateRestaurants()
    {
        // Массив Restaurant[] — типизированная коллекция начальных Entity-объектов.
        // Вложенные коллекции MenuItems сразу формируют связи one-to-many.
        return
        [
            new Restaurant
            {
                Name = "Север",
                MenuItems =
                [
                    new MenuItem { Name = "Уха по-северному", Price = 590 },
                    new MenuItem { Name = "Пельмени с олениной", Price = 720 },
                    new MenuItem { Name = "Морошковый десерт", Price = 390 },
                ],
            },
            new Restaurant
            {
                Name = "Итальянский дворик",
                MenuItems =
                [
                    new MenuItem { Name = "Маргарита", Price = 650 },
                    new MenuItem { Name = "Лазанья болоньезе", Price = 790 },
                    new MenuItem { Name = "Тирамису", Price = 450 },
                ],
            },
            new Restaurant
            {
                Name = "Восточный базар",
                MenuItems =
                [
                    new MenuItem { Name = "Плов с бараниной", Price = 680 },
                    new MenuItem { Name = "Манты", Price = 560 },
                    new MenuItem { Name = "Пахлава", Price = 320 },
                ],
            },
            new Restaurant
            {
                Name = "Зелёная кухня",
                MenuItems =
                [
                    new MenuItem { Name = "Боул с киноа", Price = 610 },
                    new MenuItem { Name = "Крем-суп из брокколи", Price = 430 },
                    new MenuItem { Name = "Яблочный крамбл", Price = 370 },
                ],
            },
        ];
    }
}

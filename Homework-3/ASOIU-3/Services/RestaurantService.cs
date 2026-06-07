using ASOIU_3.Data;
using Microsoft.EntityFrameworkCore;

namespace ASOIU_3.Services;

// Сервис инкапсулирует CRUD справочной таблицы и не смешивает работу с БД
// с кодом Console или WinForms. Это применение инкапсуляции и разделения ответственности.
internal sealed class RestaurantService
{
    private const int MaximumNameLength = 120;

    public IReadOnlyList<RestaurantListItem> GetAll()
    {
        using var context = new AppDbContext();

        // Запрос начинается с DbSet<Restaurant>, поэтому EF Core переводит эту LINQ-цепочку в SQL.
        return context.Restaurants
            .AsNoTracking()
            // Лямбда задаёт ключ сортировки: рестораны выводятся по названию.
            .OrderBy(restaurant => restaurant.Name)
            // Select проецирует Entity-модель в вспомогательный тип для интерфейса.
            // Count здесь считает связанные блюда каждого ресторана.
            .Select(restaurant => new RestaurantListItem(
                restaurant.Id,
                restaurant.Name,
                restaurant.MenuItems.Count))
            // ToList выполняет SQL-запрос и материализует результат в обобщённый список.
            .ToList();
    }

    public ServiceResult Add(string? name)
    {
        var normalizedName = NormalizeName(name);
        var validationResult = ValidateName(normalizedName);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        using var context = new AppDbContext();
        if (RestaurantNameExists(context, normalizedName))
        {
            return ServiceResult.Fail("Ресторан с таким названием уже существует.");
        }

        context.Restaurants.Add(new Models.Restaurant { Name = normalizedName });
        context.SaveChanges();
        return ServiceResult.Ok("Ресторан добавлен.");
    }

    public ServiceResult Update(int id, string? name)
    {
        var normalizedName = NormalizeName(name);
        var validationResult = ValidateName(normalizedName);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        using var context = new AppDbContext();
        // Find ищет Entity по первичному ключу и при возможности использует кэш контекста.
        var restaurant = context.Restaurants.Find(id);
        if (restaurant is null)
        {
            return ServiceResult.Fail("Ресторан с указанным идентификатором не найден.");
        }

        if (RestaurantNameExists(context, normalizedName, id))
        {
            return ServiceResult.Fail("Ресторан с таким названием уже существует.");
        }

        restaurant.Name = normalizedName;
        context.Restaurants.Update(restaurant);
        context.SaveChanges();
        return ServiceResult.Ok("Название ресторана изменено.");
    }

    public ServiceResult Delete(int id)
    {
        using var context = new AppDbContext();
        var restaurant = context.Restaurants.Find(id);
        if (restaurant is null)
        {
            return ServiceResult.Fail("Ресторан с указанным идентификатором не найден.");
        }

        // Any выполняет проверку существования связанных блюд без загрузки всей коллекции.
        // Лямбда использует внешний параметр id: это замыкание, которое EF Core
        // преобразует в параметр SQL-запроса.
        var hasMenuItems = context.MenuItems
            .Any(menuItem => menuItem.RestaurantId == id);
        if (hasMenuItems)
        {
            return ServiceResult.Fail(
                "Удаление запрещено: у ресторана есть связанные блюда.");
        }

        context.Restaurants.Remove(restaurant);
        context.SaveChanges();
        return ServiceResult.Ok("Ресторан удалён.");
    }

    private static string NormalizeName(string? name) => name?.Trim() ?? string.Empty;

    private static ServiceResult ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return ServiceResult.Fail("Название не может быть пустым.");
        }

        return name.Length > MaximumNameLength
            ? ServiceResult.Fail(
                $"Название не может быть длиннее {MaximumNameLength} символов.")
            : ServiceResult.Ok(string.Empty);
    }

    private static bool RestaurantNameExists(
        AppDbContext context,
        string name,
        int? excludedId = null)
    {
        var normalizedName = name.ToLower();

        // Лямбда замыкает сразу две внешние переменные: normalizedName и excludedId.
        // При изменении excludedId текущая запись исключается из проверки уникальности.
        return context.Restaurants.Any(
            restaurant =>
                restaurant.Name.ToLower() == normalizedName
                && (!excludedId.HasValue || restaurant.Id != excludedId.Value));
    }
}

// record — компактный вспомогательный тип результата, а не таблица базы данных.
// Он передаёт интерфейсу только нужные для списка поля и вычисленное количество блюд.
internal sealed record RestaurantListItem(int Id, string Name, int MenuItemCount);

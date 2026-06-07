using ASOIU_3.Data;
using ASOIU_3.Models;
using Microsoft.EntityFrameworkCore;

namespace ASOIU_3.Services;

// Сервис отвечает за CRUD основной таблицы блюд и централизует её бизнес-валидацию.
internal sealed class MenuItemService
{
    private const int MaximumNameLength = 120;

    public IReadOnlyList<MenuItemListItem> GetAll()
    {
        using var context = new AppDbContext();

        var menuItems = context.MenuItems
            .AsNoTracking()
            // Include с лямбдой подгружает связанный Restaurant одним запросом,
            // чтобы далее безопасно использовать название ресторана.
            .Include(menuItem => menuItem.Restaurant)
            // OrderBy сортирует блюда по значению Name.
            .OrderBy(menuItem => menuItem.Name)
            // ToList материализует EF Core-запрос; после этой строки дальнейший LINQ
            // работает уже с объектами в памяти.
            .ToList();

        return menuItems
            // Select преобразует Entity в вспомогательную строку списка.
            // Лямбда передаёт каждый элемент в общий метод Map.
            .Select(menuItem => Map(menuItem))
            .ToList();
    }

    public MenuItemListItem? GetById(int id)
    {
        using var context = new AppDbContext();

        var menuItem = context.MenuItems
            .AsNoTracking()
            .Include(item => item.Restaurant)
            // FirstOrDefault возвращает найденный объект либо null; id из параметра метода
            // захватывается лямбдой item => item.Id == id, то есть используется замыкание.
            .FirstOrDefault(item => item.Id == id);

        return menuItem is null ? null : Map(menuItem);
    }

    public IReadOnlyList<RestaurantChoice> GetRestaurantChoices()
    {
        using var context = new AppDbContext();

        return context.Restaurants
            .AsNoTracking()
            .OrderBy(restaurant => restaurant.Name)
            // Проекция не передаёт форме всю Entity-модель, а создаёт только пару Id/Name.
            .Select(restaurant => new RestaurantChoice(
                restaurant.Id,
                restaurant.Name))
            .ToList();
    }

    public ServiceResult Add(string? name, double price, int restaurantId)
    {
        var normalizedName = NormalizeName(name);
        var validationResult = Validate(normalizedName, price);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        using var context = new AppDbContext();
        // Any проверяет существование выбранного внешнего ключа до сохранения блюда.
        // Лямбда замыкает restaurantId, полученный из формы.
        if (!context.Restaurants.Any(restaurant => restaurant.Id == restaurantId))
        {
            return ServiceResult.Fail("Выбранный ресторан не найден.");
        }

        context.MenuItems.Add(new MenuItem
        {
            Name = normalizedName,
            Price = price,
            RestaurantId = restaurantId,
        });
        context.SaveChanges();
        return ServiceResult.Ok("Блюдо добавлено.");
    }

    public ServiceResult Update(
        int id,
        string? name,
        double price,
        int restaurantId)
    {
        var normalizedName = NormalizeName(name);
        var validationResult = Validate(normalizedName, price);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        using var context = new AppDbContext();
        var menuItem = context.MenuItems.Find(id);
        if (menuItem is null)
        {
            return ServiceResult.Fail("Блюдо с указанным идентификатором не найдено.");
        }

        if (!context.Restaurants.Any(restaurant => restaurant.Id == restaurantId))
        {
            return ServiceResult.Fail("Выбранный ресторан не найден.");
        }

        menuItem.Name = normalizedName;
        menuItem.Price = price;
        menuItem.RestaurantId = restaurantId;
        context.MenuItems.Update(menuItem);
        context.SaveChanges();
        return ServiceResult.Ok("Блюдо изменено.");
    }

    public ServiceResult Delete(int id)
    {
        using var context = new AppDbContext();
        var menuItem = context.MenuItems.Find(id);
        if (menuItem is null)
        {
            return ServiceResult.Fail("Блюдо с указанным идентификатором не найдено.");
        }

        context.MenuItems.Remove(menuItem);
        context.SaveChanges();
        return ServiceResult.Ok("Блюдо удалено.");
    }

    private static MenuItemListItem Map(MenuItem menuItem)
    {
        return new MenuItemListItem(
            menuItem.Id,
            menuItem.Name,
            menuItem.Price,
            menuItem.RestaurantId,
            menuItem.Restaurant.Name);
    }

    private static string NormalizeName(string? name) => name?.Trim() ?? string.Empty;

    private static ServiceResult Validate(string name, double price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return ServiceResult.Fail("Название блюда не может быть пустым.");
        }

        if (name.Length > MaximumNameLength)
        {
            return ServiceResult.Fail(
                $"Название не может быть длиннее {MaximumNameLength} символов.");
        }

        // Проверка price < 0 выполняет требование ТЗ: отрицательная цена недопустима.
        if (!double.IsFinite(price) || price < 0)
        {
            return ServiceResult.Fail(
                "Цена должна быть конечным неотрицательным числом.");
        }

        return ServiceResult.Ok(string.Empty);
    }
}

// Эти record-типы не являются таблицами БД. Они играют роль ViewModel/DTO:
// передают в интерфейс подготовленные данные и скрывают Entity от слоя отображения.
internal sealed record MenuItemListItem(
    int Id,
    string Name,
    double Price,
    int RestaurantId,
    string RestaurantName);

internal sealed record RestaurantChoice(int Id, string Name);

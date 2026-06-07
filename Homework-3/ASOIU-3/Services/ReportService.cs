using ASOIU_3.Data;
using Microsoft.EntityFrameworkCore;

namespace ASOIU_3.Services;

// Отчётный сервис строит все разделы отчёта декларативными LINQ-запросами.
internal sealed class ReportService
{
    public ReportData Generate()
    {
        using var context = new AppDbContext();

        var menuItems = context.MenuItems
            .AsNoTracking()
            // Include нужен для доступа к названию связанного ресторана.
            .Include(menuItem => menuItem.Restaurant)
            // Лямбда передаётся OrderBy как функция выбора ключа сортировки.
            .OrderBy(menuItem => menuItem.Name)
            // Пока запрос работает с DbSet, EF Core переводит LINQ в SQL.
            // ToList выполняет SQL и материализует результат в List<MenuItem>.
            .ToList();

        var fullList = menuItems
            // Select создаёт для каждой Entity отдельный объект строки отчёта,
            // содержащий только данные, которые нужно показать пользователю.
            .Select(menuItem => new FullListReportRow(
                menuItem.Name,
                menuItem.Restaurant.Name,
                menuItem.Price))
            .ToList();

        var countValues = context.MenuItems
            .AsNoTracking()
            // GroupBy группирует блюда по названию ресторана.
            .GroupBy(menuItem => menuItem.Restaurant.Name)
            // group.Key — ключ текущей группы, то есть название ресторана.
            // Count считает количество блюд, вошедших в эту группу.
            // new { ... } создаёт анонимный вспомогательный тип внутри метода:
            // он удобен как промежуточная форма результата LINQ-запроса.
            .Select(group => new
            {
                RestaurantName = group.Key,
                Count = group.Count(),
            })
            // Результаты группировки сортируются по названию ресторана.
            .OrderBy(row => row.RestaurantName)
            // Здесь запрос к DbSet выполняется в SQLite и превращается в список объектов.
            .ToList();

        var counts = countValues
            // После материализации Select преобразует анонимные объекты
            // в именованный тип CategoryCountReportRow для передачи интерфейсу.
            .Select(row => new CategoryCountReportRow(
                row.RestaurantName,
                row.Count))
            .ToList();

        var averagePriceValues = context.MenuItems
            .AsNoTracking()
            // Для каждой группы ресторана будет рассчитан отдельный агрегат.
            .GroupBy(menuItem => menuItem.Restaurant.Name)
            .Select(group => new
            {
                RestaurantName = group.Key,
                // Average вычисляет среднюю цену. Лямбда menuItem => menuItem.Price
                // передаётся LINQ как функция выбора числового значения.
                AveragePrice = group.Average(menuItem => menuItem.Price),
            })
            // OrderByDescending располагает рестораны от большей средней цены к меньшей.
            .OrderByDescending(row => row.AveragePrice)
            .ToList();

        var averagePrices = averagePriceValues
            .Select(row => new AveragePriceReportRow(
                row.RestaurantName,
                row.AveragePrice))
            .ToList();

        return new ReportData(fullList, counts, averagePrices);
    }
}

// ReportData — модель всего отчёта: она объединяет три обобщённые коллекции
// IReadOnlyList<T> и передаёт их слою отображения одним объектом.
internal sealed record ReportData(
    IReadOnlyList<FullListReportRow> FullList,
    IReadOnlyList<CategoryCountReportRow> Counts,
    IReadOnlyList<AveragePriceReportRow> AveragePrices);

// Отчётные строки вынесены в отдельные вспомогательные типы, потому что структура
// результата запроса отличается от Entity-моделей таблиц и не должна сохраняться в БД.
internal sealed record FullListReportRow(
    string MenuItemName,
    string RestaurantName,
    double Price);

internal sealed record CategoryCountReportRow(
    string RestaurantName,
    int Count);

internal sealed record AveragePriceReportRow(
    string RestaurantName,
    double AveragePrice);

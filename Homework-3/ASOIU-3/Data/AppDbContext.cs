using ASOIU_3.Models;
using Microsoft.EntityFrameworkCore;

namespace ASOIU_3.Data;

/// <summary>
/// Контекст Entity Framework Core для базы данных ресторанов.
/// </summary>
// AppDbContext наследуется от DbContext. Это пример наследования в ООП:
// класс получает готовые механизмы подключения, запросов и сохранения EF Core.
public sealed class AppDbContext : DbContext
{
    /// <summary>
    /// Инициализирует контекст с настройками подключения по умолчанию.
    /// </summary>
    public AppDbContext()
    {
    }

    /// <summary>
    /// Инициализирует контекст с переданными настройками.
    /// </summary>
    /// <param name="options">Настройки контекста базы данных.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Получает набор ресторанов.
    /// </summary>
    // DbSet<T> — обобщённый тип EF Core. Здесь T = Restaurant,
    // поэтому свойство представляет таблицу Restaurants и позволяет выполнять LINQ-запросы.
    public DbSet<Restaurant> Restaurants => Set<Restaurant>();

    /// <summary>
    /// Получает набор блюд меню.
    /// </summary>
    // Здесь T = MenuItem: DbSet<MenuItem> соответствует основной таблице MenuItems.
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // UseSqlite выбирает провайдер SQLite и файл app.db как источник данных.
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
    }

    /// <inheritdoc />
    // Переопределённый метод OnModelCreating настраивает модель БД поверх соглашений EF Core.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Лямбда entity => ... передаёт EF Core код настройки сущности Restaurant.
        modelBuilder.Entity<Restaurant>(entity =>
        {
            // Лямбда restaurant => restaurant.Name указывает на свойство без строкового имени,
            // поэтому переименование свойства контролируется компилятором.
            entity.Property(restaurant => restaurant.Name)
                .HasMaxLength(120)
                .IsRequired();

            entity.HasIndex(restaurant => restaurant.Name)
                .IsUnique();
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.Property(menuItem => menuItem.Name)
                .HasMaxLength(120)
                .IsRequired();

            // HasOne, WithMany и HasForeignKey явно задают связь one-to-many:
            // у блюда один ресторан, у ресторана много блюд, внешний ключ — RestaurantId.
            entity.HasOne(menuItem => menuItem.Restaurant)
                .WithMany(restaurant => restaurant.MenuItems)
                .HasForeignKey(menuItem => menuItem.RestaurantId)
                // Restrict запрещает каскадное удаление ресторана вместе с его блюдами.
                // Пользователь должен сначала явно обработать связанные записи.
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

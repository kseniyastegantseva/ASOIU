using System.Text;
using ASOIU_3.Data;
using ASOIU_3.UI;

namespace ASOIU_3;

/// Точка входа приложения для работы с ресторанами и блюдами.
public static class Program
{
    /// Создаёт базу данных, заполняет её начальными данными и запускает приложение.
    /// Возвращает код завершения приложения.
    public static int Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            // Сначала общий инициализатор подготавливает SQLite,
            // затем управление передаётся слою пользовательского интерфейса.
            DatabaseInitializer.Initialize();
            var application = new ConsoleApplication();
            application.Run();
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(
                $"Не удалось запустить приложение: {exception.Message}");
            return 1;
        }
    }
}

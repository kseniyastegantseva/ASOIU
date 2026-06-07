using ASOIU_3.Data;

namespace ASOIU_3.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        // Обе версии интерфейса используют один и тот же инициализатор и слой данных.
        DatabaseInitializer.Initialize();
        // Application.Run запускает цикл сообщений и показывает главное окно.
        Application.Run(new MainForm());
    }
}

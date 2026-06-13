using System;
using System.Windows.Forms;
using WinFormsApp1.Data;
using WinFormsApp1.Forms;

namespace WinFormsApp1
{
    /// <summary>
    /// Главный класс приложения, содержащий точку входа
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// Создаёт базу данных (если её нет) и запускает главную форму.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Создание базы данных и начальных данных при первом запуске
            using (var context = new AppDbContext())
            {
                context.Database.EnsureCreated();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
using System.Windows.Forms;

namespace WinFormsApp1.Forms
{
    /// <summary>
    /// Главная форма с навигацией по разделам приложения
    /// </summary>
    public class MainForm : Form
    {
        private Button btnGenres, btnBooks, btnReport;

        /// <summary>
        /// Конструктор главной формы: создаёт кнопки меню и задаёт их обработчики
        /// </summary>
        public MainForm()
        {
            this.ClientSize = new System.Drawing.Size(280, 280);
            this.Text = "Главное меню";

            btnGenres = new Button { Text = "Жанры", Location = new System.Drawing.Point(50, 50), Size = new System.Drawing.Size(150, 40) };
            btnGenres.Click += (s, e) => new GenresForm().ShowDialog();

            btnBooks = new Button { Text = "Книги", Location = new System.Drawing.Point(50, 110), Size = new System.Drawing.Size(150, 40) };
            btnBooks.Click += (s, e) => new BooksForm().ShowDialog();

            btnReport = new Button { Text = "Отчёт", Location = new System.Drawing.Point(50, 170), Size = new System.Drawing.Size(150, 40) };
            btnReport.Click += (s, e) => new ReportForm().ShowDialog();

            Controls.Add(btnGenres);
            Controls.Add(btnBooks);
            Controls.Add(btnReport);
        }
    }
}
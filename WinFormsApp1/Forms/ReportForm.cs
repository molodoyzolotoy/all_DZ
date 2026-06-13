using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using WinFormsApp1.Data;

namespace WinFormsApp1.Forms
{
    /// <summary>
    /// Форма для отображения трёх аналитических отчётов по книгам
    /// </summary>
    public class ReportForm : Form
    {
        private DataGridView dgvFullList, dgvCountByGenre, dgvAvgPages;

        /// <summary>
        /// Конструктор: создаёт три DataGridView и заполняет их отчётами
        /// </summary>
        public ReportForm()
        {
            this.ClientSize = new System.Drawing.Size(600, 420);
            this.Text = "Отчёт";

            dgvFullList = new DataGridView { Location = new System.Drawing.Point(12, 12), Size = new System.Drawing.Size(560, 150), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            dgvCountByGenre = new DataGridView { Location = new System.Drawing.Point(12, 180), Size = new System.Drawing.Size(270, 150), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            dgvAvgPages = new DataGridView { Location = new System.Drawing.Point(300, 180), Size = new System.Drawing.Size(272, 150), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };

            Controls.Add(dgvFullList);
            Controls.Add(dgvCountByGenre);
            Controls.Add(dgvAvgPages);

            LoadReports();
        }

        /// <summary>
        /// Загружает данные для трёх отчётов (LINQ to Entities)
        /// </summary>
        private void LoadReports()
        {
            using (var context = new AppDbContext())
            {
                // Отчёт 1: полный список книг с названиями жанров (Include + OrderBy)
                var fullList = context.Books
                    .Include(b => b.Genre)
                    .OrderBy(b => b.Name)
                    .Select(b => new { Название = b.Name, Жанр = b.Genre != null ? b.Genre.Name : "", Страницы = b.Pages })
                    .ToList();
                dgvFullList.DataSource = fullList;

                // Отчёт 2: количество книг по жанрам (GroupBy + Count)
                var countByGenre = context.Books
                    .GroupBy(b => b.Genre != null ? b.Genre.Name : "Без жанра")
                    .Select(g => new { Жанр = g.Key, Количество = g.Count() })
                    .OrderBy(r => r.Жанр)
                    .ToList();
                dgvCountByGenre.DataSource = countByGenre;

                // Отчёт 3: среднее количество страниц по жанрам (GroupBy + Average + OrderByDescending)
                var avgPages = context.Books
                    .Where(b => b.Genre != null)
                    .GroupBy(b => b.Genre!.Name)
                    .Select(g => new { Жанр = g.Key, Среднее = Math.Round(g.Average(b => b.Pages), 2) })
                    .OrderByDescending(r => r.Среднее)
                    .ToList();
                dgvAvgPages.DataSource = avgPages;
            }
        }
    }
}
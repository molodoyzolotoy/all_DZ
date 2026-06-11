using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using WinFormsApp1.Data;

namespace WinFormsApp1.Forms
{
    public class ReportForm : Form
    {
        private DataGridView dgvFullList, dgvCountByGenre, dgvAvgPages;

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

        private void LoadReports()
        {
            using (var context = new AppDbContext())
            {
                var fullList = context.Books
                    .Include(b => b.Genre)
                    .OrderBy(b => b.Name)
                    .Select(b => new { Название = b.Name, Жанр = b.Genre != null ? b.Genre.Name : "", Страницы = b.Pages })
                    .ToList();
                dgvFullList.DataSource = fullList;

                var countByGenre = context.Books
                    .GroupBy(b => b.Genre != null ? b.Genre.Name : "Без жанра")
                    .Select(g => new { Жанр = g.Key, Количество = g.Count() })
                    .OrderBy(r => r.Жанр)
                    .ToList();
                dgvCountByGenre.DataSource = countByGenre;

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
using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using WinFormsApp1.Data;
using WinFormsApp1.Models;

namespace WinFormsApp1.Forms
{
    /// <summary>
    /// Форма для управления книгами (CRUD)
    /// </summary>
    public class BooksForm : Form
    {
        private DataGridView grid;
        private Button btnAdd, btnEdit, btnDelete;

        /// <summary>
        /// Конструктор: создаёт элементы управления и загружает список книг
        /// </summary>
        public BooksForm()
        {
            this.Text = "Список книг";
            this.ClientSize = new System.Drawing.Size(700, 450);

            grid = new DataGridView { Location = new System.Drawing.Point(10, 10), Size = new System.Drawing.Size(660, 300), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            btnAdd = new Button { Text = "Добавить", Location = new System.Drawing.Point(10, 330), Size = new System.Drawing.Size(100, 40) };
            btnEdit = new Button { Text = "Редактировать", Location = new System.Drawing.Point(120, 330), Size = new System.Drawing.Size(100, 40) };
            btnDelete = new Button { Text = "Удалить", Location = new System.Drawing.Point(230, 330), Size = new System.Drawing.Size(100, 40) };

            btnAdd.Click += (s, e) => AddBook();
            btnEdit.Click += (s, e) => EditBook();
            btnDelete.Click += (s, e) => DeleteBook();

            Controls.Add(grid);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);

            LoadData();
        }

        /// <summary>
        /// Загружает список книг с названиями жанров (через Include) в DataGridView
        /// </summary>
        private void LoadData()
        {
            using (var db = new AppDbContext())
            {
                var list = db.Books.Include(x => x.Genre).OrderBy(x => x.Name)
                    .Select(x => new { x.Id, x.Name, Жанр = x.Genre != null ? x.Genre.Name : "", x.Pages })
                    .ToList();
                grid.DataSource = list;
            }
        }

        /// <summary>
        /// Добавление новой книги (диалоги ввода названия, страниц, выбор жанра)
        /// </summary>
        private void AddBook()
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Название книги:", "Добавление");
            if (string.IsNullOrWhiteSpace(name)) return;

            string pagesStr = Microsoft.VisualBasic.Interaction.InputBox("Количество страниц:", "Страницы");
            if (!int.TryParse(pagesStr, out int pages) || pages < 0)
            {
                MessageBox.Show("Введите неотрицательное число.");
                return;
            }

            using (var db = new AppDbContext())
            {
                var genres = db.Genres.OrderBy(x => x.Name).ToList();
                if (genres.Count == 0)
                {
                    MessageBox.Show("Сначала создайте жанр.");
                    return;
                }
                string menu = string.Join("\n", genres.Select((x, i) => $"{i + 1}. {x.Name}"));
                string choice = Microsoft.VisualBasic.Interaction.InputBox($"Выберите жанр:\n{menu}", "Жанр");
                if (!int.TryParse(choice, out int idx) || idx < 1 || idx > genres.Count) return;

                db.Books.Add(new Book { Name = name, Pages = pages, GenreId = genres[idx - 1].Id });
                db.SaveChanges();
            }
            LoadData();
        }

        /// <summary>
        /// Редактирование выбранной книги (название, страницы, жанр)
        /// </summary>
        private void EditBook()
        {
            if (grid.CurrentRow == null) return;
            int id = (int)grid.CurrentRow.Cells["Id"].Value;

            using (var db = new AppDbContext())
            {
                var book = db.Books.Find(id);
                if (book == null) return;

                string newName = Microsoft.VisualBasic.Interaction.InputBox("Новое название:", "Редактирование", book.Name);
                if (!string.IsNullOrWhiteSpace(newName)) book.Name = newName;

                string pagesStr = Microsoft.VisualBasic.Interaction.InputBox("Новое количество страниц:", "Страницы", book.Pages.ToString());
                if (int.TryParse(pagesStr, out int newPages) && newPages >= 0) book.Pages = newPages;

                var genres = db.Genres.OrderBy(x => x.Name).ToList();
                string menu = string.Join("\n", genres.Select((x, i) => $"{i + 1}. {x.Name}"));
                string choice = Microsoft.VisualBasic.Interaction.InputBox($"Выберите новый жанр:\n{menu}", "Жанр");
                if (int.TryParse(choice, out int idx) && idx >= 1 && idx <= genres.Count)
                    book.GenreId = genres[idx - 1].Id;

                db.SaveChanges();
            }
            LoadData();
        }

        /// <summary>
        /// Удаление выбранной книги (с подтверждением)
        /// </summary>
        private void DeleteBook()
        {
            if (grid.CurrentRow == null) return;
            int id = (int)grid.CurrentRow.Cells["Id"].Value;
            if (MessageBox.Show("Удалить книгу?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var db = new AppDbContext())
                {
                    var b = db.Books.Find(id);
                    if (b != null) db.Books.Remove(b);
                    db.SaveChanges();
                }
                LoadData();
            }
        }
    }
}
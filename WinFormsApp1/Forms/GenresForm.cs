using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using WinFormsApp1.Data;
using WinFormsApp1.Models;

namespace WinFormsApp1.Forms
{
    /// <summary>
    /// Форма для управления справочником «Жанры» (CRUD)
    /// </summary>
    public class GenresForm : Form
    {
        private DataGridView dgvGenres;
        private Button btnAdd, btnEdit, btnDelete;

        /// <summary>
        /// Конструктор: создаёт элементы управления и загружает список жанров
        /// </summary>
        public GenresForm()
        {
            this.ClientSize = new System.Drawing.Size(500, 420);
            this.Text = "Жанры";

            dgvGenres = new DataGridView { Location = new System.Drawing.Point(12, 12), Size = new System.Drawing.Size(460, 300), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            btnAdd = new Button { Text = "Добавить", Location = new System.Drawing.Point(12, 330), Size = new System.Drawing.Size(100, 40) };
            btnEdit = new Button { Text = "Редактировать", Location = new System.Drawing.Point(130, 330), Size = new System.Drawing.Size(100, 40) };
            btnDelete = new Button { Text = "Удалить", Location = new System.Drawing.Point(248, 330), Size = new System.Drawing.Size(100, 40) };

            btnAdd.Click += (s, e) => AddGenre();
            btnEdit.Click += (s, e) => EditGenre();
            btnDelete.Click += (s, e) => DeleteGenre();

            Controls.Add(dgvGenres);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);

            LoadGenres();
        }

        /// <summary>
        /// Загружает список всех жанров из БД и отображает в DataGridView
        /// </summary>
        private void LoadGenres()
        {
            using (var context = new AppDbContext())
            {
                var genres = context.Genres.OrderBy(g => g.Name).ToList();
                dgvGenres.DataSource = genres;
                if (dgvGenres.Columns["Books"] != null)
                    dgvGenres.Columns["Books"].Visible = false;
            }
        }

        /// <summary>
        /// Добавление нового жанра (диалог ввода названия)
        /// </summary>
        private void AddGenre()
        {
            string newName = Microsoft.VisualBasic.Interaction.InputBox("Введите название жанра:", "Добавление");
            if (string.IsNullOrWhiteSpace(newName))
            {
                MessageBox.Show("Название не может быть пустым.");
                return;
            }
            using (var context = new AppDbContext())
            {
                context.Genres.Add(new Genre { Name = newName });
                context.SaveChanges();
            }
            LoadGenres();
        }

        /// <summary>
        /// Редактирование выбранного жанра
        /// </summary>
        private void EditGenre()
        {
            if (dgvGenres.CurrentRow == null) return;
            int id = (int)dgvGenres.CurrentRow.Cells["Id"].Value;
            string oldName = dgvGenres.CurrentRow.Cells["Name"].Value?.ToString() ?? "";
            string newName = Microsoft.VisualBasic.Interaction.InputBox("Редактирование жанра", "Измените название", oldName);
            if (string.IsNullOrWhiteSpace(newName)) return;
            using (var context = new AppDbContext())
            {
                var genre = context.Genres.Find(id);
                if (genre != null)
                {
                    genre.Name = newName;
                    context.SaveChanges();
                }
            }
            LoadGenres();
        }

        /// <summary>
        /// Удаление выбранного жанра (с проверкой наличия связанных книг)
        /// </summary>
        private void DeleteGenre()
        {
            if (dgvGenres.CurrentRow == null) return;
            int id = (int)dgvGenres.CurrentRow.Cells["Id"].Value;
            using (var context = new AppDbContext())
            {
                var genre = context.Genres.Include(g => g.Books).FirstOrDefault(g => g.Id == id);
                if (genre == null) return;
                if (genre.Books.Any())
                {
                    MessageBox.Show("Нельзя удалить жанр, так как есть связанные книги.", "Запрет удаления");
                    return;
                }
                if (MessageBox.Show("Удалить жанр?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    context.Genres.Remove(genre);
                    context.SaveChanges();
                    LoadGenres();
                }
            }
        }
    }
}
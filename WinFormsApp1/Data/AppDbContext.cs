using Microsoft.EntityFrameworkCore;
using WinFormsApp1.Models;

namespace WinFormsApp1.Data
{
    /// <summary>
    /// Контекст базы данных для работы с жанрами и книгами (Entity Framework Core)
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Таблица жанров
        /// </summary>
        public DbSet<Genre> Genres { get; set; }

        /// <summary>
        /// Таблица книг
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Настройка подключения к базе данных SQLite
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }

        /// <summary>
        /// Заполнение базы данных начальными данными при создании
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Жанры (не менее 4)
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Роман" },
                new Genre { Id = 2, Name = "Фантастика" },
                new Genre { Id = 3, Name = "Детектив" },
                new Genre { Id = 4, Name = "Поэзия" }
            );

            // Книги (не менее 12)
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, GenreId = 1, Name = "Война и мир", Pages = 1300 },
                new Book { Id = 2, GenreId = 1, Name = "Анна Каренина", Pages = 800 },
                new Book { Id = 3, GenreId = 1, Name = "Преступление и наказание", Pages = 600 },
                new Book { Id = 4, GenreId = 2, Name = "1984", Pages = 328 },
                new Book { Id = 5, GenreId = 2, Name = "Дюна", Pages = 688 },
                new Book { Id = 6, GenreId = 2, Name = "Нейромант", Pages = 300 },
                new Book { Id = 7, GenreId = 3, Name = "Убийство в Восточном экспрессе", Pages = 256 },
                new Book { Id = 8, GenreId = 3, Name = "Десять негритят", Pages = 270 },
                new Book { Id = 9, GenreId = 3, Name = "Собака Баскервилей", Pages = 210 },
                new Book { Id = 10, GenreId = 4, Name = "Евгений Онегин", Pages = 400 },
                new Book { Id = 11, GenreId = 4, Name = "Божественная комедия", Pages = 480 },
                new Book { Id = 12, GenreId = 4, Name = "Лирика Ахматовой", Pages = 150 }
            );
        }
    }
}









using System.Collections.Generic;

namespace WinFormsApp1.Models
{
    /// <summary>
    /// Жанр книги (справочная таблица, сторона "один")
    /// </summary>
    public class Genre
    {
        /// <summary>
        /// Уникальный идентификатор жанра (первичный ключ)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название жанра (например, "Роман", "Фантастика")
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Навигационное свойство: список книг данного жанра
        /// </summary>
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}




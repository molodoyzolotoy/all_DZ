namespace WinFormsApp1.Models
{
    /// <summary>
    /// Книга (основная таблица, сторона "много")
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Уникальный идентификатор книги (первичный ключ)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Внешний ключ – идентификатор жанра
        /// </summary>
        public int GenreId { get; set; }

        /// <summary>
        /// Навигационное свойство: жанр книги
        /// </summary>
        public Genre? Genre { get; set; }

        /// <summary>
        /// Название книги
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Количество страниц (не может быть отрицательным)
        /// </summary>
        public int Pages { get; set; }
    }
}
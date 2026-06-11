namespace WinFormsApp1.Models
{
    public class Book
    {
        public int Id { get; set; }
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Pages { get; set; }
    }
}
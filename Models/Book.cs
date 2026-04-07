namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int PublishYear { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public int QuantityInStock { get; set; }

        // Внешние ключи
        public int AuthorId { get; set; }
        public int GenreId { get; set; }

        // Навигационные свойства
        public virtual Author? Author { get; set; }
        public virtual Genre? Genre { get; set; }
    }
}
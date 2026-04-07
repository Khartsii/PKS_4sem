// ViewModels/BookViewModel.cs
namespace LibraryApp.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public int PublishYear { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string GenreName { get; set; } = string.Empty;
        public int QuantityInStock { get; set; }
    }
}
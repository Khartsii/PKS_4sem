// MainWindow.xaml.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models;
using LibraryApp.ViewModels;

namespace LibraryApp
{
    public partial class MainWindow : Window
    {
        private LibraryDbContext _context = null!;
        private List<BookViewModel> _allBooks = null!;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded!;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _context = new LibraryDbContext();
                _context.Database.EnsureCreated();

                LoadBooks();
                LoadFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void LoadFilters()
        {
            var genres = _context.Genres.ToList();
            var allGenres = new List<Genre> { new Genre { Id = 0, Name = "Все жанры", Description = "" } };
            allGenres.AddRange(genres);
            cmbGenreFilter.ItemsSource = allGenres;
            cmbGenreFilter.DisplayMemberPath = "Name";
            cmbGenreFilter.SelectedValuePath = "Id";
            cmbGenreFilter.SelectedIndex = 0;

            var authors = _context.Authors.ToList();
            var allAuthors = new List<Author> { new Author { Id = 0, FirstName = "Все", LastName = "авторы", BirthDate = DateTime.Now, Country = "" } };
            allAuthors.AddRange(authors);
            cmbAuthorFilter.ItemsSource = allAuthors;
            cmbAuthorFilter.DisplayMemberPath = "FullName";
            cmbAuthorFilter.SelectedValuePath = "Id";
            cmbAuthorFilter.SelectedIndex = 0;
        }

        private void LoadBooks()
        {
            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author != null ? b.Author.FullName : "Неизвестен",
                    PublishYear = b.PublishYear,
                    ISBN = b.ISBN,
                    GenreName = b.Genre != null ? b.Genre.Name : "Неизвестен",
                    QuantityInStock = b.QuantityInStock
                })
                .ToList();

            _allBooks = books;
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_allBooks == null)
            {
                return;
            }
            var filteredBooks = _allBooks.AsEnumerable();

            if (cmbGenreFilter.SelectedItem is Genre selectedGenre && selectedGenre.Id > 0)
            {
                filteredBooks = filteredBooks.Where(b => b.GenreName == selectedGenre.Name);
            }

            if (cmbAuthorFilter.SelectedItem is Author selectedAuthor && selectedAuthor.Id > 0)
            {
                filteredBooks = filteredBooks.Where(b => b.AuthorName == selectedAuthor.FullName);
            }

            dgBooks.ItemsSource = filteredBooks.ToList();
        }

        private void CmbGenreFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void CmbAuthorFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void BtnResetFilter_Click(object sender, RoutedEventArgs e)
        {
            cmbGenreFilter.SelectedIndex = 0;
            cmbAuthorFilter.SelectedIndex = 0;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new BookDialog(_context, null);
            if (dialog.ShowDialog() == true)
            {
                LoadBooks();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgBooks.SelectedItem is BookViewModel selectedBook)
            {
                var book = _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Genre)
                    .FirstOrDefault(b => b.Id == selectedBook.Id);

                if (book != null)
                {
                    var dialog = new BookDialog(_context, book);
                    if (dialog.ShowDialog() == true)
                    {
                        LoadBooks();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgBooks.SelectedItem is BookViewModel selectedBook)
            {
                var result = MessageBox.Show($"Удалить книгу '{selectedBook.Title}'?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var book = _context.Books.Find(selectedBook.Id);
                    if (book != null)
                    {
                        _context.Books.Remove(book);
                        _context.SaveChanges();
                        LoadBooks();
                        MessageBox.Show("Книга удалена", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
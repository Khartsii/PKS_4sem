// BookDialog.xaml.cs
using System;
using System.Linq;
using System.Windows;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class BookDialog : Window
    {
        private LibraryDbContext _context;
        private Book? _editingBook;

        public BookDialog(LibraryDbContext context, Book? book = null)
        {
            InitializeComponent();
            _context = context;
            _editingBook = book;

            LoadComboBoxes();

            if (book != null)
            {
                Title = "Редактирование книги";
                LoadBookData();
            }
            else
            {
                Title = "Добавление книги";
            }
        }

        private void LoadComboBoxes()
        {
            cmbAuthor.ItemsSource = _context.Authors.ToList();
            cmbGenre.ItemsSource = _context.Genres.ToList();
        }

        private void LoadBookData()
        {
            if (_editingBook != null)
            {
                txtTitle.Text = _editingBook.Title;
                cmbAuthor.SelectedValue = _editingBook.AuthorId;
                txtYear.Text = _editingBook.PublishYear.ToString();
                txtISBN.Text = _editingBook.ISBN;
                cmbGenre.SelectedValue = _editingBook.GenreId;
                txtQuantity.Text = _editingBook.QuantityInStock.ToString();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите название книги", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbAuthor.SelectedItem == null)
            {
                MessageBox.Show("Выберите автора", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(txtYear.Text, out int year) || year < 0 || year > DateTime.Now.Year)
            {
                MessageBox.Show("Введите корректный год издания", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtISBN.Text))
            {
                MessageBox.Show("Введите ISBN", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbGenre.SelectedItem == null)
            {
                MessageBox.Show("Выберите жанр", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Введите корректное количество", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_editingBook == null)
            {
                var newBook = new Book
                {
                    Title = txtTitle.Text,
                    AuthorId = (int)cmbAuthor.SelectedValue,
                    PublishYear = year,
                    ISBN = txtISBN.Text,
                    GenreId = (int)cmbGenre.SelectedValue,
                    QuantityInStock = quantity
                };
                _context.Books.Add(newBook);
            }
            else
            {
                _editingBook.Title = txtTitle.Text;
                _editingBook.AuthorId = (int)cmbAuthor.SelectedValue;
                _editingBook.PublishYear = year;
                _editingBook.ISBN = txtISBN.Text;
                _editingBook.GenreId = (int)cmbGenre.SelectedValue;
                _editingBook.QuantityInStock = quantity;
            }

            _context.SaveChanges();
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
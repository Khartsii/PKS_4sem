using System;
using System.Linq;
using System.Windows;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class GenreDialog : Window
    {
        private LibraryDbContext _context;
        private Genre? _editingGenre;

        public GenreDialog(LibraryDbContext context, Genre? genre = null)
        {
            InitializeComponent();
            _context = context;
            _editingGenre = genre;

            if (genre != null)
            {
                Title = "Редактирование жанра";
                LoadGenreData();
            }
            else
            {
                Title = "Добавление жанра";
            }
        }

        private void LoadGenreData()
        {
            if (_editingGenre != null)
            {
                txtName.Text = _editingGenre.Name;
                txtDescription.Text = _editingGenre.Description;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название жанра", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Введите описание жанра", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_editingGenre == null)
            {
                var newGenre = new Genre
                {
                    Name = txtName.Text,
                    Description = txtDescription.Text
                };
                _context.Genres.Add(newGenre);
            }
            else
            {
                _editingGenre.Name = txtName.Text;
                _editingGenre.Description = txtDescription.Text;
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
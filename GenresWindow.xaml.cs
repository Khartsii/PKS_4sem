using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class GenresWindow : Window
    {
        private LibraryDbContext _context;

        public GenresWindow(LibraryDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadGenres();
        }

        private void LoadGenres()
        {
            dgGenres.ItemsSource = _context.Genres.ToList();
        }

        private void BtnAddGenre_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new GenreDialog(_context, null);
            if (dialog.ShowDialog() == true)
            {
                LoadGenres();
            }
        }

        private void BtnEditGenre_Click(object sender, RoutedEventArgs e)
        {
            if (dgGenres.SelectedItem is Genre selectedGenre)
            {
                var dialog = new GenreDialog(_context, selectedGenre);
                if (dialog.ShowDialog() == true)
                {
                    LoadGenres();
                }
            }
            else
            {
                MessageBox.Show("Выберите жанр для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            if (dgGenres.SelectedItem is Genre selectedGenre)
            {
                // Проверяем, есть ли у жанра книги
                var hasBooks = _context.Books.Any(b => b.GenreId == selectedGenre.Id);
                if (hasBooks)
                {
                    MessageBox.Show("Нельзя удалить жанр, у которого есть книги", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = MessageBox.Show($"Удалить жанр '{selectedGenre.Name}'?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _context.Genres.Remove(selectedGenre);
                    _context.SaveChanges();
                    LoadGenres();
                    MessageBox.Show("Жанр удален", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите жанр для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
using System;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class AuthorsWindow : Window
    {
        private LibraryDbContext _context;

        public AuthorsWindow(LibraryDbContext context)
        {
            InitializeComponent();
            _context = context;
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            dgAuthors.ItemsSource = _context.Authors.ToList();
        }

        private void BtnAddAuthor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AuthorDialog(_context, null);
            if (dialog.ShowDialog() == true)
            {
                LoadAuthors();
            }
        }

        private void BtnEditAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (dgAuthors.SelectedItem is Author selectedAuthor)
            {
                var dialog = new AuthorDialog(_context, selectedAuthor);
                if (dialog.ShowDialog() == true)
                {
                    LoadAuthors();
                }
            }
            else
            {
                MessageBox.Show("Выберите автора для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDeleteAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (dgAuthors.SelectedItem is Author selectedAuthor)
            {
                // Проверяем, есть ли у автора книги
                var hasBooks = _context.Books.Any(b => b.AuthorId == selectedAuthor.Id);
                if (hasBooks)
                {
                    MessageBox.Show("Нельзя удалить автора, у которого есть книги", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = MessageBox.Show($"Удалить автора '{selectedAuthor.FullName}'?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _context.Authors.Remove(selectedAuthor);
                    _context.SaveChanges();
                    LoadAuthors();
                    MessageBox.Show("Автор удален", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите автора для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
using System;
using System.Linq;
using System.Windows;
using LibraryApp.Data;
using LibraryApp.Models;

namespace LibraryApp
{
    public partial class AuthorDialog : Window
    {
        private LibraryDbContext _context;
        private Author? _editingAuthor;

        public AuthorDialog(LibraryDbContext context, Author? author = null)
        {
            InitializeComponent();
            _context = context;
            _editingAuthor = author;

            if (author != null)
            {
                Title = "Редактирование автора";
                LoadAuthorData();
            }
            else
            {
                Title = "Добавление автора";
            }
        }

        private void LoadAuthorData()
        {
            if (_editingAuthor != null)
            {
                txtFirstName.Text = _editingAuthor.FirstName;
                txtLastName.Text = _editingAuthor.LastName;
                dpBirthDate.SelectedDate = _editingAuthor.BirthDate;
                txtCountry.Text = _editingAuthor.Country;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Введите имя автора", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Введите фамилию автора", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dpBirthDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату рождения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCountry.Text))
            {
                MessageBox.Show("Введите страну", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_editingAuthor == null)
            {
                var newAuthor = new Author
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    BirthDate = dpBirthDate.SelectedDate.Value,
                    Country = txtCountry.Text
                };
                _context.Authors.Add(newAuthor);
            }
            else
            {
                _editingAuthor.FirstName = txtFirstName.Text;
                _editingAuthor.LastName = txtLastName.Text;
                _editingAuthor.BirthDate = dpBirthDate.SelectedDate.Value;
                _editingAuthor.Country = txtCountry.Text;
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
// Data/LibraryDbContext.cs
using System;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Models;

namespace LibraryApp.Data
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=LibraryDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация для Book
            // Уникальный индекс для ISBN
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique();

            // Уникальный индекс для комбинации (Title, AuthorId, PublishYear)
            modelBuilder.Entity<Book>()
                .HasIndex(b => new { b.Title, b.AuthorId, b.PublishYear })
                .IsUnique()
                .HasDatabaseName("IX_Book_Title_Author_Year");
                
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ISBN)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.PublishYear)
                    .IsRequired();

                entity.Property(e => e.QuantityInStock)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.HasOne(e => e.Author)
                    .WithMany(a => a.Books)
                    .HasForeignKey(e => e.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Genre)
                    .WithMany(g => g.Books)
                    .HasForeignKey(e => e.GenreId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Конфигурация для Author
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.BirthDate)
                    .IsRequired();
            });

            // Конфигурация для Genre
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            // Добавление начальных данных
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Добавление жанров
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Роман", Description = "Литературный жанр" },
                new Genre { Id = 2, Name = "Детектив", Description = "Остросюжетный жанр" },
                new Genre { Id = 3, Name = "Фантастика", Description = "Научно-фантастические произведения" }
            );

            // Добавление авторов
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FirstName = "Фёдор", LastName = "Достоевский", BirthDate = new DateTime(1821, 11, 11), Country = "Россия" },
                new Author { Id = 2, FirstName = "Лев", LastName = "Толстой", BirthDate = new DateTime(1828, 9, 9), Country = "Россия" },
                new Author { Id = 3, FirstName = "Артур", LastName = "Конан Дойл", BirthDate = new DateTime(1859, 5, 22), Country = "Великобритания" }
            );
        }
    }
}
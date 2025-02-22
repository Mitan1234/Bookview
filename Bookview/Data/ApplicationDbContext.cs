using Bookview.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookview.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser> // ✅ Użycie AppUser
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet dla każdej tabeli w bazie danych
        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Wywołanie domyślnej konfiguracji Identity

            // 📌 Relacja: Jeden użytkownik może mieć wiele recenzji
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Nie można usunąć użytkownika, jeśli są jego recenzje

            // 📌 Relacja: Jedna książka może mieć wiele recenzji
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade); // Usunięcie książki powoduje usunięcie recenzji

            // 📌 Relacja: Książka należy do jednej kategorii
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.SetNull); // Jeśli kategoria zostanie usunięta, książki zostają, ale tracą kategorię
        }
    }
}

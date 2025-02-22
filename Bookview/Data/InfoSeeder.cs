using Bookview.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookview.Data
{
    public static class InfoSeeder
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();

                if (await dbContext.Database.CanConnectAsync())
                {
                    await SeedRolesAsync(roleManager);
                    await SeedUsersAsync(userManager);
                    await SeedCategoriesAsync(dbContext);
                    await SeedBooksAsync(dbContext);
                    await SeedReviewsAsync(dbContext, userManager);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Błąd podczas inicjalizacji danych: {ex.Message}");
            }
        }

        // 📌 2️⃣ Dodawanie Ról
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "admin", "user" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        // 📌 3️⃣ Dodawanie Użytkowników
        private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            var usersToCreate = new List<(AppUser User, string Password, string Role)>
            {
                (new AppUser { UserName = "admin@portal.pl", Email = "admin@portal.pl", EmailConfirmed = true, FirstName = "Ewa", LastName = "Admin" }, "Admin123!", "admin"),
                (new AppUser { UserName = "user1@portal.pl", Email = "user1@portal.pl", EmailConfirmed = true, FirstName = "Piotr", LastName = "Pisarski" }, "User123!", "user"),
                (new AppUser { UserName = "user2@portal.pl", Email = "user2@portal.pl", EmailConfirmed = true, FirstName = "Anna", LastName = "Autorska" }, "User123!", "user")
            };

            foreach (var (user, password, role) in usersToCreate)
            {
                if (await userManager.FindByEmailAsync(user.Email) == null)
                {
                    var result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }

        // 📌 4️⃣ Dodawanie Kategorii (Gatunki książek)
        private static async Task SeedCategoriesAsync(ApplicationDbContext dbContext)
        {
            if (!await dbContext.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Fantastyka", Description = "Książki z elementami fantastyki", Icon = "book" },
                    new Category { Name = "Kryminał", Description = "Powieści detektywistyczne", Icon = "shield" },
                    new Category { Name = "Horror", Description = "Opowieści grozy", Icon = "ghost" },
                    new Category { Name = "Biografia", Description = "Życiorysy znanych ludzi", Icon = "user" },
                    new Category { Name = "Nauka", Description = "Książki popularnonaukowe", Icon = "flask" }
                };

                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.SaveChangesAsync();
            }
        }

        // 📌 5️⃣ Dodawanie Książek
        private static async Task SeedBooksAsync(ApplicationDbContext dbContext)
        {
            if (!await dbContext.Books.AnyAsync())
            {
                var books = new List<Book>
                {
                    new Book { Title = "Władca Pierścieni", Author = "J.R.R. Tolkien", Description = "Klasyka fantasy.", CoverImage = "lotr.jpg", CategoryId = 1 },
                    new Book { Title = "Zbrodnia i kara", Author = "Fiodor Dostojewski", Description = "Psychologiczna powieść kryminalna.", CoverImage = "crime.jpg", CategoryId = 2 },
                    new Book { Title = "Dracula", Author = "Bram Stoker", Description = "Klasyczna historia wampira.", CoverImage = "dracula.jpg", CategoryId = 3 },
                    new Book { Title = "Steve Jobs", Author = "Walter Isaacson", Description = "Biografia założyciela Apple.", CoverImage = "jobs.jpg", CategoryId = 4 },
                    new Book { Title = "Teoria względności", Author = "Albert Einstein", Description = "Fundamenty współczesnej fizyki.", CoverImage = "relativity.jpg", CategoryId = 5 }
                };

                await dbContext.Books.AddRangeAsync(books);
                await dbContext.SaveChangesAsync();
            }
        }

        // 📌 6️⃣ Dodawanie Recenzji (Bez ocen gwiazdkowych)
        private static async Task SeedReviewsAsync(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            if (!await dbContext.Reviews.AnyAsync())
            {
                var user1 = await userManager.FindByEmailAsync("user1@portal.pl");
                var user2 = await userManager.FindByEmailAsync("user2@portal.pl");

                if (user1 != null && user2 != null)
                {
                    var reviews = new List<Review>
                    {
                        new Review { Content = "Świetna książka!", AddedDate = DateTime.Now.AddDays(-3), UserId = user1.Id, BookId = 1 },
                        new Review { Content = "Bardzo wciągająca fabuła!", AddedDate = DateTime.Now.AddDays(-5), UserId = user2.Id, BookId = 2 },
                        new Review { Content = "Nie polecam, nie dla mnie.", AddedDate = DateTime.Now.AddDays(-7), UserId = user1.Id, BookId = 3 },
                        new Review { Content = "Doskonała biografia!", AddedDate = DateTime.Now.AddDays(-10), UserId = user2.Id, BookId = 4 },
                        new Review { Content = "Warto przeczytać, ale trudna książka.", AddedDate = DateTime.Now.AddDays(-12), UserId = user1.Id, BookId = 5 }
                    };

                    await dbContext.Reviews.AddRangeAsync(reviews);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}

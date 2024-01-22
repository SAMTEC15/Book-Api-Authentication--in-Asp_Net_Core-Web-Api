using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Persistence.Seeder
{
    public static class AddDbInitializer
    {
        // public static void Seed(IApplicationBuilder applicationBuilder)
        public static void Seed(ModelBuilder modelBuilder)
        {

            //if (!modelBuilder.Model.GetEntityTypes().Any(e => e.ClrType == typeof(Book)))
            //{
                modelBuilder.Entity<Book>().HasData(
                new Book()
                {
                    Id = 1,
                    Title = "Apart",
                    Description = "Things fall Apart",
                    IsRead = true,
                    DateRead = DateTime.Now.AddDays(-10),
                    Rate = 4,
                    Genre = "Biography",
                    //Author = "First Author",
                    CoverUrl = "https....",
                    DateAdded = DateTime.Now,
                    PublisherId = 3,
                    
                },
                   new Book()
                   {
                       Id = 2,
                       Title = "Gener",
                       Description = "The Rise of a General",
                       IsRead = false,
                       DateRead = DateTime.Now.AddDays(-15),
                       Rate = 5,
                       Genre = "Biography",
                      // Author = "Second Author",
                       CoverUrl = "https....",
                       DateAdded = DateTime.Now,
                       PublisherId = 2,
                   },
                    new Book()
                    {
                        Id = 3,
                        Title = "General",
                        Description = "The Rise of a General",
                        IsRead = false,
                        DateRead = DateTime.Now.AddDays(-15),
                        Rate = 5,
                        Genre = "Biography",
                        //Author = "Second Author",
                        CoverUrl = "https....",
                        DateAdded = DateTime.Now,
                        PublisherId = 1,
                        

                    });
            //}
        }

       /* private static void SeedUserRoles(ModelBuilder modelBuilder)
        {
            var roleManger = modelBuilder.
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = UserRole.Admin, NormalizedName = UserRole.Admin.ToUpper() },
                new IdentityRole { Id = "2", Name = UserRole.Publisher, NormalizedName = UserRole.Publisher.ToUpper() },
                new IdentityRole { Id = "3", Name = UserRole.Author, NormalizedName = UserRole.Author.ToUpper() },
                new IdentityRole { Id = "4", Name = UserRole.User, NormalizedName = UserRole.User.ToUpper() }
            );
        }*/
        public static async Task SeedUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Seed roles
            if (!await roleManager.RoleExistsAsync(UserRole.Admin))
            {
                var role = new IdentityRole(UserRole.Admin);
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync(UserRole.Author))
            {
                var role = new IdentityRole(UserRole.Author);
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync(UserRole.Publisher))
            {
                var role = new IdentityRole(UserRole.Publisher);
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync(UserRole.User))
            {
                var role = new IdentityRole(UserRole.User);
                await roleManager.CreateAsync(role);
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
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
                    Author = "First Author",
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
                       Author = "Second Author",
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
                        Author = "Second Author",
                        CoverUrl = "https....",
                        DateAdded = DateTime.Now,
                        PublisherId = 1,
                        

                    });
            //}
        }
    }
}

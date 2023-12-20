using Microsoft.EntityFrameworkCore;
using MyBook.Domain.Models;
using MyBook.Persistence.Seeder;

namespace MyBook.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BookAuthor>()
                .HasOne(u => u.Book)
                .WithMany(u => u.BookAuthors)
                .HasForeignKey(bi => bi.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(u => u.Author)
                .WithMany(u => u.BookAuthors)
                .HasForeignKey(bi => bi.AuthorId);

       

            // Call the SeedData method from the DataSeeder class
            //AddDbInitializer.Seed(modelBuilder);
        }
    }
}

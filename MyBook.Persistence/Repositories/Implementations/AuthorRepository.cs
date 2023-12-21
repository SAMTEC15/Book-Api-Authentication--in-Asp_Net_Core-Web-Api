using Microsoft.EntityFrameworkCore;
using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using MyBook.Persistence.Repositories.Interfaces;

namespace MyBook.Persistence.Repositories.Implementations
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AuthorRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Author> AddAuthor(AuthorAddDto authorAddDto)
        {
            if (authorAddDto == null)
            {
                return null;
            }
            var author = new Author
            {
                FirstName = authorAddDto.FirstName,
                LastName = authorAddDto.LastName,
                //BookAuthors = authorAddDto.BookAuthors
            };
            await _applicationDbContext.Authors.AddAsync(author);
            await _applicationDbContext.SaveChangesAsync();
            return author;
        }
        public async Task<Author> DeleteById(int? id)
        {
            var author = await _applicationDbContext.Authors.FirstOrDefaultAsync(u => u.Id == id);
            if (author == null)
            {
                return null;
            }
            _applicationDbContext.Authors.Remove(author);
            await _applicationDbContext.SaveChangesAsync();
            return author;
        }
        public async Task<IEnumerable<Author>> GetAllAuthor() => await _applicationDbContext.Authors.ToListAsync();
        public async Task<Author> GetById(int? id) => await _applicationDbContext.Authors.FirstOrDefaultAsync(u => u.Id == id);
        public async Task<AuthorAddDto> UpdateAuthor(int id, AuthorAddDto authorAddDto)
        {
            var existingPublisher = await _applicationDbContext.Authors.FirstOrDefaultAsync(u => u.Id.Equals(id));
            if (existingPublisher == null)
            {
                return null;
            }
            existingPublisher.FirstName = authorAddDto.FirstName;
            existingPublisher.LastName = authorAddDto.LastName;

            await _applicationDbContext.SaveChangesAsync();
            return authorAddDto;
        }

        public async Task<AuthorWithBooksDto> GetBookWithAuthor(int? id)
        {
            var author = await _applicationDbContext.Authors.Where(u => u.Id == id).Select(u => new AuthorWithBooksDto
            {
                LastName = u.LastName,
                FirstName = u.FirstName,
                BookTitles = u.BookAuthors.Select(u => u.Book.Title).ToList(),
            }).FirstOrDefaultAsync();
            return author;
        }

    }
}
using MyBook.Domain.Dto;
using MyBook.Domain.Models;

namespace MyBook.Persistence.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Author> GetById(int? id);
        Task<IEnumerable<Author>> GetAllAuthor();
        Task<Author> DeleteById(int? id);
        Task<AuthorAddDto> UpdateAuthor(int id, AuthorAddDto authorAddDto);
        Task<Author> AddAuthor(AuthorAddDto authorAddDto);
        Task<AuthorWithBooksDto> GetBookWithAuthor(int? id);
    }
}

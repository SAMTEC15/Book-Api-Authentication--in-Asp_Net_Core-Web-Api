using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Application.Interfaces
{
    public interface IAuthorSerice
    {
        Task<Author> AddAuthorAsync(AuthorAddDto authorAddDto);
        Task<IEnumerable<Author>> GetAllAuthor();
        Task<Author> GetAuthorByIdAsync(int? id);
        Task<Author> UpdateAuthor(int id, AuthorAddDto authorAddDto);
        Task<Author> DeleteAuthor(int? id);
        Task<AuthorWithBooksDto> GetBookWithAuthor(int? id);
    }
}

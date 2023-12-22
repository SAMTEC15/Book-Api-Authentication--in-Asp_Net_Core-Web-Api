using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Persistence.Repositories.Interfaces
{
    public interface IBooksRepository
    {
        Task<BookAuthorsReturnDto> GetById(int? id);
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> DeleteById(int? id);
        Task<BookAddDto> UpdateBook(int id, BookAddDto book);
        Task<Book> AddBook(BookAddDto books);
        Task<List<Book>> GetAllBooksAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null,
            bool isAscending = true, int pageNumber = 1, int pageSize = 10);

    }
}

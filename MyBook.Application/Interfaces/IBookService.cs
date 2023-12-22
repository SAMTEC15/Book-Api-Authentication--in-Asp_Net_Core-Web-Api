using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Application.Interfaces
{
    public interface IBookService
    {
        Task<Book> AddBookAsync(BookAddDto books);
        Task<IEnumerable<Book>> GetAllBooks();
        Task<BookAuthorsReturnDto> GetBookByIdAsync(int? id);
        Task<Book> UpdateBook(int id, BookAddDto books);
        Task<Book> DeleteBook(int? id);
        Task<List<Book>> GetAllAsync(string? filterOn, string? filterQuery, string? sortBy, bool isAsending, int pageNumber, int pageSize);
    }
}

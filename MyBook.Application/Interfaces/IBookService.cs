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
        Task<Book> GetBookByIdAsync(int? id);
        Task<Book> UpdateBook(int id, BookAddDto books);
        Task<Book> DeleteBook(int? id);
    }
}

using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Persistence.Repositories
{
    public interface IBooksRepository 
    {
        Task<Book> GetById(int? id);
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> DeleteById(int? id);
        Task<BookAddDto> UpdateBook(int id, BookAddDto book);
        Task<Book> AddBook(Book book);

    }
}

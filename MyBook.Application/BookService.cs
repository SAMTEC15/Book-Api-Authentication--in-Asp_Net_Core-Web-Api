using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using MyBook.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace MyBook.Application
{
    public class BookService : IBookService
    {
        private readonly IBooksRepository _booksRepository;

        public BookService(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public async Task<Book> AddBookAsync(BookAddDto books)
        {
            var book = new Book()
            {
                IsRead = books.IsRead,
                Author = books.Author,
                CoverUrl = books.CoverUrl,
                DateRead = books.IsRead ? books.DateRead.Value : null,
                Description = books.Description,
                Genre = books.Genre,
                Rate = books.IsRead ? books.Rate.Value : null,
                Title = books.Title,
            };
            _booksRepository.AddBook(book);
            return await Task.FromResult(book);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            var allBooks = await _booksRepository.GetAllBooks();
            return allBooks;
        }

        public async Task<Book> GetBookByIdAsync(int? id)
        {
            if (id < 0 || id == null)
            {
                return null;
            }
            var getBook = await _booksRepository.GetById(id);
            if (getBook == null)
            {
                return null;
            }
            return getBook;
        }

        public async Task<Book> UpdateBook(int id, BookAddDto books)
        {
            var update = await _booksRepository.UpdateBook(id, books);
            if (update == null)
            {
                return null;
            }
            var book = new Book
            {
                Id = id,
                Author = books.Author,
                CoverUrl = books.CoverUrl,
                Description = books.Description,
                Genre = books.Genre,
                Rate = books.Rate,
                Title = books.Title,
                IsRead = books.IsRead,
                DateRead = books.DateRead,
            };
            return book;
        }
        public async Task<Book> DeleteBook(int? id)
        {
            if(id < 0 || id == null)
            {
                return null;
            }
            var book =await _booksRepository.DeleteById(id);
            return book;
        }
    }
}

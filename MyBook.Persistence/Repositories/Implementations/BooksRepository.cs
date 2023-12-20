using Microsoft.EntityFrameworkCore;
using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using MyBook.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyBook.Persistence.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public BooksRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Book> AddBook(Book book)
        {
            if(book == null)
            {
                return null;
            }          
           await _applicationDbContext.Books.AddAsync(book);
            await _applicationDbContext.SaveChangesAsync();         
            return book;
        }
        public async Task<Book> DeleteById(int? id)
        {
           var existingBook = await _applicationDbContext.Books.FirstOrDefaultAsync(u => u.Id == id);
            if(existingBook == null)
            {
                return null;
            }
            _applicationDbContext.Books.Remove(existingBook);
           await _applicationDbContext.SaveChangesAsync();
            return existingBook;
        }
        public async Task<IEnumerable<Book>> GetAllBooks() => await _applicationDbContext.Books.ToListAsync(); 
        public async Task<Book> GetById(int? id) => await  _applicationDbContext.Books.FirstOrDefaultAsync(u => u.Id == id);      
        public async Task<BookAddDto> UpdateBook(int id, BookAddDto book)
        {            
            var bookCheck = await _applicationDbContext.Books.FirstOrDefaultAsync(u => u.Id.Equals(id));
            if (book == null || bookCheck == null)
            {
                return null;
            }

            bookCheck.IsRead = book.IsRead;
            bookCheck.Author = book.Author;
            bookCheck.CoverUrl = book.CoverUrl;
            bookCheck.DateRead = book.IsRead ? book.DateRead.Value : null;
            bookCheck.Description = book.Description;
            bookCheck.Genre = book.Genre;
            bookCheck.Rate = book.IsRead ? book.Rate.Value : null;
            bookCheck.Title = book.Title;
            
            await _applicationDbContext.SaveChangesAsync();
            return book;
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBook.Application.Interfaces;
using MyBook.Domain.Dto;
using MyBook.Domain.Models;

namespace MyBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookAddDto bookAddDto)
        {
            if (bookAddDto == null)
            {
                return BadRequest("InPut not in correct state");
            }
            await _bookService.AddBookAsync(bookAddDto);
            return Ok(bookAddDto);
        }

        [HttpGet("get/all/books")]
        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _bookService.GetAllBooks();
        }

        [HttpGet("get/book/byId/{id}")]
        public  async Task<IActionResult> GetBook(int id)
        {
           var getBook = await _bookService.GetBookByIdAsync(id);
            if(getBook == null)
            {
                return NotFound("Book not found");
            }
            return Ok(getBook);
        }

        [HttpPut("update-book-by-id/{id}")]
        public  async Task<IActionResult> UpdateBook(int id, [FromBody] BookAddDto bookAddDto)
        {
            var updateBook = await _bookService.UpdateBook(id, bookAddDto);
            if(updateBook == null)
            {
                return BadRequest("Book not found.");
            }
            return Ok(updateBook);
        }

        [HttpDelete("delete-book-by-id/{id}")]
        public async Task<IActionResult> Delete(int? id)
        { 
            var book = await _bookService.DeleteBook(id);
            if(book == null)
            {
                return NotFound("Book not found.");
            }
           
            return Ok(book);
        }
    }
}

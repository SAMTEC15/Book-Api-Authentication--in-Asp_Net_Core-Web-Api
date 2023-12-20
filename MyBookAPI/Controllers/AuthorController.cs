using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBook.Application;
using MyBook.Domain.Dto;
using MyBook.Persistence.Repositories;

namespace MyBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorSerice _authorSerice;

        public AuthorController(IAuthorSerice authorSerice)
        {
            _authorSerice = authorSerice;
        }

        [HttpPost("add-author")]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorAddDto authorAddDto)
        {            
          var addAuthor =  await _authorSerice.AddAuthorAsync(authorAddDto);
            if(addAuthor == null)
            {
                return BadRequest("Author was not add");
            }
            return  Ok("Author Added success");
        }
    }
}

using MyBook.Application.Interfaces;
using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using MyBook.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Application.Implementations
{
    public class AuthorService : IAuthorSerice
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<Author> AddAuthorAsync(AuthorAddDto authorAddDto)
        {           
            if (authorAddDto == null)
            {
                return null;
            }
            var addedAuthor = await _authorRepository.AddAuthor(authorAddDto);
            return addedAuthor;
        }
        public async Task<Author> DeleteAuthor(int? id)
        {
            if (id == null || id < 0)
            {
                return null;
            }
            var deleInfo = await _authorRepository.DeleteById(id);
            if (deleInfo == null)
            {
                return null;
            }
            return deleInfo;
        }
        public async Task<IEnumerable<Author>> GetAllAuthor() => await _authorRepository.GetAllAuthor();
        public async Task<Author> GetAuthorByIdAsync(int? id)
        {
            if (id == null || id < 0)
            {
                return null;
            }
            var gottenAuthor = await _authorRepository.GetById(id);
            if (gottenAuthor == null)
            {
                return null;
            }
            return gottenAuthor;
        }
        public async Task<Author> UpdateAuthor(int id, AuthorAddDto authorAddDto)
        {
            var update = await _authorRepository.UpdateAuthor(id, authorAddDto);
            if (update == null)
            {
                return null;
            }
            var updated = new Author
            {
                FirstName = authorAddDto.FirstName,
                LastName = authorAddDto.LastName,
            };
            return updated;
        }
    }
}

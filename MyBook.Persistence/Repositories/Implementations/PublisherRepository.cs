using Microsoft.EntityFrameworkCore;
using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using MyBook.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Persistence.Repositories.Implementations
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public PublisherRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }       
        public async Task<Publisher> AddPublisher(Publisher publisher)
        {
            if (publisher == null)
            {
                return null;
            }
            await _applicationDbContext.Publishers.AddAsync(publisher);
            await _applicationDbContext.SaveChangesAsync();
            return publisher;
        }
        public async Task<Publisher> DeleteById(int? id)
        {
            var publisher = await _applicationDbContext.Publishers.FirstOrDefaultAsync(u => u.Id == id);
            if (publisher == null)
            {
                return null;
            }
            _applicationDbContext.Publishers.Remove(publisher);
            await _applicationDbContext.SaveChangesAsync();
            return publisher;
        }
        public async Task<List<Publisher>> GetAllPublisher() => await _applicationDbContext.Publishers.ToListAsync();
        public async Task<Publisher> GetById(int? id) => await _applicationDbContext.Publishers.FirstOrDefaultAsync(u => u.Id == id);
        public async Task<PublisherAddDto> UpdatePublisher(int id, PublisherAddDto publisherAddDto)
        {
            var existingAuthor = await _applicationDbContext.Publishers.FirstOrDefaultAsync(u => u.Id.Equals(id));
            if (existingAuthor == null)
            {
                return null;
            }
            existingAuthor.CEO = publisherAddDto.CEO;
            existingAuthor.Revenue = publisherAddDto.Revenue;
            existingAuthor.Website = publisherAddDto.Website;
            existingAuthor.Address = publisherAddDto.Address;
            existingAuthor.ContactInformation = publisherAddDto.ContactInformation;
            existingAuthor.Country = publisherAddDto.Country;
            existingAuthor.Name = publisherAddDto.Name;
            existingAuthor.FoundingYear = publisherAddDto.FoundingYear;

            await _applicationDbContext.SaveChangesAsync();
            return publisherAddDto;
        }
    }
}

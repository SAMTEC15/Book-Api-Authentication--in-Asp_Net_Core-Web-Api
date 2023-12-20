using MyBook.Application.Interfaces;
using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using MyBook.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace MyBook.Application.Implementations
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;
        public PublisherService(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }
        public Task<Publisher> AddPublisherAsync(PublisherAddDto publisherAddDto)
        {
            var publisher = new Publisher
            {
                Address = publisherAddDto.Address,
                CEO = publisherAddDto.CEO,
                Name = publisherAddDto.Name,
                ContactInformation = publisherAddDto.ContactInformation,
                Country = publisherAddDto.Country,
                FoundingYear = publisherAddDto.FoundingYear,
                Revenue = publisherAddDto.Revenue,
                Website = publisherAddDto.Website,
            };
            _publisherRepository.AddPublisher(publisher);
            return Task.FromResult(publisher);
        }
        public async Task<Publisher> DeletePublisher(int? id)
        {
            if (id == null || id < 0)
            {
                return null;
            }
            var deleInfo = await _publisherRepository.DeleteById(id);
            if (deleInfo == null)
            {
                return null;
            }
            return deleInfo;
        }
        public Task<IEnumerable<Publisher>> GetAllPublisher() => _publisherRepository.GetAllPublisher();
        public async Task<Publisher> GetPublisherByIdAsync(int? id)
        {
            if (id == null || id < 0)
            {
                return null;
            }
            var gottenBook = await _publisherRepository.GetById(id);
            if (gottenBook == null)
            {
                return null;
            }
            return gottenBook;
        }
        public async Task<Publisher> UpdatePublisher(int id, PublisherAddDto publisherAddDto)
        {
            var update = await _publisherRepository.UpdatePublisher(id, publisherAddDto);
            if (update == null)
            {
                return null;
            }
            var publisher = new Publisher
            {
                Id = id,
                Address = publisherAddDto.Address,
                CEO = publisherAddDto.CEO,
                ContactInformation = publisherAddDto.ContactInformation,
                Country = publisherAddDto.Country,
                FoundingYear = publisherAddDto.FoundingYear,
                Name = publisherAddDto.Name,
                Revenue = publisherAddDto.Revenue,
                Website = publisherAddDto.Website,
            };
            return publisher;
        }
    }
}

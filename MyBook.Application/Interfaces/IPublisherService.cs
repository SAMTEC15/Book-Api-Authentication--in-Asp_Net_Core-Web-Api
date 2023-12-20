using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Application.Interfaces
{
    public interface IPublisherService
    {
        Task<Publisher> AddPublisherAsync(PublisherAddDto publisherAddDto);
        Task<IEnumerable<Publisher>> GetAllPublisher();
        Task<Publisher> GetPublisherByIdAsync(int? id);
        Task<Publisher> UpdatePublisher(int id, PublisherAddDto publisherAddDto);
        Task<Publisher> DeletePublisher(int? id);
    }
}

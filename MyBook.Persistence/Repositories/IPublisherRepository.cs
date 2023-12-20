using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Persistence.Repositories
{
    public interface IPublisherRepository
    {
        Task<Publisher> GetById(int? id);
        Task<IEnumerable<Publisher>> GetAllPublisher();
        Task<Publisher> DeleteById(int? id);
        Task<PublisherAddDto> UpdatePublisher(int id, PublisherAddDto publisherAddDto);
        Task<Publisher> AddPublisher(Publisher publisher);
    }
}

using MyBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Persistence.Repositories.Interfaces
{
    public interface IImageRepository 
    {
        Task<Image> Upload(Image image);
    }
}

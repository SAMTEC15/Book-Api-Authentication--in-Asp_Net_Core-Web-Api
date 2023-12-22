using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyBook.Domain.Models;
using MyBook.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Persistence.Repositories.Implementations
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageRepository(ApplicationDbContext applicationDbContext, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads",
                $"{image.FileName}{image.FileExtension}");

            //Upload Image to loacal Path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Uploads/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            await _applicationDbContext.Images.AddAsync(image);
            await _applicationDbContext.SaveChangesAsync();

            return image;
        }
    }
}

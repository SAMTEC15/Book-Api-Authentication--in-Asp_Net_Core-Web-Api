using Microsoft.AspNetCore.Mvc;
using MyBook.Application.Interfaces;
using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using MyBook.Persistence.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Application.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        public async Task<Image> UploadImageAsync(ImageUploadRequestDto imageUploadRequestDto)
        {
            //ValidateFileUpload(imageUploadRequestDto);
            if (ValidateFileUpload(imageUploadRequestDto) == null)
            {
                var imageDomain = new Image
                {
                    File = imageUploadRequestDto.File,
                    FileExtension = Path.GetExtension(imageUploadRequestDto.File.FileName),
                    FileSizeInBytes = imageUploadRequestDto.File.Length,
                    FileName = imageUploadRequestDto.FileName,
                    FileDescription = imageUploadRequestDto.FileDescription,
                };
                await _imageRepository.Upload(imageDomain);
                return imageDomain;
            }
            return null;
        }


       /* private void ValidateFileUpload(ImageUploadRequestDto imageUploadRequestDto)
        {
            var allowExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowExtensions.Contains(Path.GetExtension(imageUploadRequestDto.File.FileName).ToLower()))
            {
               return ("file", "Unsupported file extension");
            }

            if (imageUploadRequestDto.File.Length > 2097152)
            {
                return ("file", "File size must be less than 2MB");

            }
        }*/

        public (string fieldName, string errorMessage)? ValidateFileUpload(ImageUploadRequestDto imageUploadRequestDto)
        {
            var allowExtensions = new string[] { ".jpg", ".jpeg", ".png", ".pdf", StringComparison.OrdinalIgnoreCase.ToString()};
            var fileExtension = Path.GetExtension(imageUploadRequestDto.File.FileName)?.ToLower();

            // Check if the file extension is supported
            if (string.IsNullOrEmpty(fileExtension) || !allowExtensions.Contains(fileExtension))
            {
                return ("file", "Unsupported file extension");
            }

            // Check if the file size exceeds 2MB
            const int maxSizeInBytes = 2 * 1024 * 1024; // 2MB in bytes
            if (imageUploadRequestDto.File.Length > maxSizeInBytes)
            {
                return ("file", "File size must be less than 2MB");
            }

            // File upload is valid
            return null;
        }

       
    }
}

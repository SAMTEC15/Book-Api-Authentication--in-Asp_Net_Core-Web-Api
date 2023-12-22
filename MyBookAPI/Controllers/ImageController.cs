using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBook.Application.Implementations;
using MyBook.Application.Interfaces;
using MyBook.Domain.Dto;

namespace MyBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto)
        {            
            if (ModelState.IsValid)
            {
              var imageDomain = await _imageService.UploadImageAsync(imageUploadRequestDto);
                if (imageDomain == null)
                    return BadRequest("Image fails to upload");
                /*var imageDomain = new Image
                {
                    File = imageUploadRequestDto.File,
                    FileExtension = Path.GetExtension(imageUploadRequestDto.File.FileName),
                    FileSizeInBytes = imageUploadRequestDto.File.Length,
                    FileName = imageUploadRequestDto.FileName,
                    FileDescription = imageUploadRequestDto.FileDescription,
                };
                await _imageService.Upload(imageDomain);*/
                return Ok(imageDomain);
            }
            return BadRequest(ModelState);
        }
    }
}

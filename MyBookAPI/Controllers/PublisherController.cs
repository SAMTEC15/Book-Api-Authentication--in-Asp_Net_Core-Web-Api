using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBook.Application.Interfaces;
using MyBook.Domain.Dto;

namespace MyBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpPost("add-publisher")]
        public async Task<IActionResult> AddPublisher([FromBody] PublisherAddDto publisherAddDto)
        {
            var addPublisher = await _publisherService.AddPublisherAsync(publisherAddDto);
            if (addPublisher == null)
            {
                return BadRequest("Publisher was not add");
            }
            return Ok("Publisher Added success");
        }
    }
}

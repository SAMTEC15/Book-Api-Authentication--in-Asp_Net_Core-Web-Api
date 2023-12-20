using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBook.Application.Implementations;
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

        [HttpGet("get-publisher-by-id/{id}")]
        public async Task<IActionResult> GetPublisherById(int id)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id);
            if (publisher == null)
            {
                return BadRequest("Publisher with the given Id notFound or Something went wrong");
            }
            return Ok(publisher);
        }

        [HttpGet("get-all-publisher")]
        public async Task<IActionResult> GetAllPublisher()
        {
            var publisher = await _publisherService.GetAllPublisher();
            if (publisher == null)
            {
                return BadRequest("Not found");
            }
            return Ok(publisher);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePublisherById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please check your input");
            }
            var publisher = await _publisherService.DeletePublisher(id);
            if (publisher == null)
            {
                return BadRequest("Publisher with the given Id not found or Something went wrong");
            }
            return Ok(publisher);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePublisherById(int id, [FromBody] PublisherAddDto publisherAddDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please check your input");
            }
            var publisher = await _publisherService.UpdatePublisher(id, publisherAddDto);
            if (publisher == null)
            {
                return BadRequest("Publisher with the given Id not found or Something went wrong");
            }
            return Ok(publisher);
        }
    }
}

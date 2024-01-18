using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyBook.Application.Interfaces;
using MyBook.Domain;
using MyBook.Domain.Dto;

namespace MyBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("User-registration")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var result = _authenticationService.Register(registerDto);
                if(result.Result.Succeeded)
                    return Ok(result.Result);              
            }
            return BadRequest();
        }
    }
}

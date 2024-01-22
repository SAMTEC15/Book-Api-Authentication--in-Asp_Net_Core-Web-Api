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
                var result = await _authenticationService.Register(registerDto);
                if(result.Succeeded)
                    return Ok(result);              
            }
            return BadRequest();
        }   

        [HttpPost("User-login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.Login(loginDto);
                if (result.Succeeded)
                    return Ok(result); 
            }
            return BadRequest();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequestDto tokenRequestDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.RefreshToken(tokenRequestDto);
                if (result != null)
                    return Ok(result);
            }
            return BadRequest();
        }
    }
}

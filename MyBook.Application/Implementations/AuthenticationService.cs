using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MyBook.Application.Interfaces;
using MyBook.Domain;
using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using MyBook.Persistence;

namespace MyBook.Application.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            ApplicationDbContext applicationDbContext, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
            _configuration = configuration;
        }

        public async Task<ApiResponse<string>> Register(RegisterDto registerDto)
        {
            var userExist = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExist != null)
                return new ApiResponse<string>(false, "Email already in use", 400, new List<string>());

            var user = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if(!result.Succeeded)
                return new ApiResponse<string>(false, "User Registration failed", 400, new List<string>());
           
            return new ApiResponse<string>(true, "Registration done successfully", 200, new List<string>());

        }
    }
}

using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyBook.Application.Interfaces;
using MyBook.Domain;
using MyBook.Domain.Dto;
using MyBook.Domain.Models;
using MyBook.Persistence;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyBook.Application.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IConfiguration _configuration;

        //RefreshToken (This service was also registered in the program class
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            ApplicationDbContext applicationDbContext, IConfiguration configuration,
             TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
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
            if (!result.Succeeded)
                return new ApiResponse<string>(false, "User Registration failed", 400, new List<string>());

            switch (registerDto.Role)
            {
                case "Admin":
                    await _userManager.AddToRoleAsync(user, registerDto.Role);
                    break;
                case "Author":
                    await _userManager.AddToRoleAsync(user, UserRole.Author);
                    break;
                case "Publisher":
                    await _userManager.AddToRoleAsync(user, UserRole.Publisher);
                    break;
              default:
                    await _userManager.AddToRoleAsync(user, UserRole.User);
                    break;
            }

            return new ApiResponse<string>(true, "Registration successfull", 200, new List<string>());

        }

        public async Task<ApiResponse<AuthResultDto>> Login(LoginDto loginDto)
        {
            var checkUser = await _userManager.FindByEmailAsync(loginDto.Email);
            if (checkUser == null)
                return new ApiResponse<AuthResultDto>(false, "Email not found", 400, new List<string>());
            await _userManager.CheckPasswordAsync(checkUser, loginDto.Password);
            var tokenValue = await GenerateJwtTokenAsync(checkUser, "");
            return new ApiResponse<AuthResultDto>(true, "Login successfully", 200, tokenValue, new List<string>());

        }

        private async Task<AuthResultDto> GenerateJwtTokenAsync(ApplicationUser user, string existingRefreshToken)
        {
            var authCliams = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Ad User Roles
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authCliams.Add(new Claim(ClaimTypes.Role, userRole));
            }


            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var token = new JwtSecurityToken
                (
                issuer: _configuration["JwtSettings:ValidIssuer"],
                audience: _configuration["JwtSettings:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(10),
                claims: authCliams,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new RefreshToken();
            if (string.IsNullOrEmpty(existingRefreshToken))
            {
                refreshToken = new RefreshToken()
                {
                    JwtId = token.Id,
                    IsRevoked = false,
                    UserId = user.Id,
                    DateAdded = DateTime.UtcNow,
                    DateExpire = DateTime.UtcNow.AddMonths(6),
                    Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString(), //GenerateRefreshToken()
                };
                await _applicationDbContext.RefreshTokens.AddAsync(refreshToken);
                await _applicationDbContext.SaveChangesAsync();
            }


            var response = new AuthResultDto()
            {
                RefreshToken = (string.IsNullOrEmpty(existingRefreshToken)) ?
                refreshToken.Token : existingRefreshToken,
                //Token = jwtToken,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpireAt = token.ValidTo
            };
            return response;
        }

        public async Task<ApiResponse<AuthResultDto>> RefreshToken(TokenRequestDto tokenRequest)
        {

            var result = await VerifyAndGenerateTokenAsync(tokenRequest);
            if (result == null)

                return null;

            return result;
        }

        private async Task<ApiResponse<AuthResultDto>> VerifyAndGenerateTokenAsync(TokenRequestDto tokenRequest)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                //Check 1: Check JWT token format
                var tokenInVerification = jwtTokenHandler
                     .ValidateToken(tokenRequest.Token,
                     _tokenValidationParameters,
                     out var validatedToken);

                /*            var tokenValidationParameters = _tokenValidationParameters.Clone(); // Create a clone to avoid modifying the original parameters
                            tokenValidationParameters.ValidateLifetime = false; // Disable lifetime validation for now
                            var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, tokenValidationParameters, out var validatedToken);
                */

                //Check 2: Encryption algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals
                        (SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)                       
                    return new ApiResponse<AuthResultDto>(false, "failed", 400, new List<string>());
                }
                //Check 3: Validate expiry date
                var utcExpiryDate = long.Parse(tokenInVerification.Claims
                    .FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTimeInUTC(utcExpiryDate);
                if (expiryDate > DateTime.UtcNow)                  
                return new ApiResponse<AuthResultDto>(false, "Token has not expired yet!", 400, new List<string>());

                //Check 4: Refresh token exists in the DB
                var dbRefreshToken = await _applicationDbContext.RefreshTokens.FirstOrDefaultAsync(u => u.Token == tokenRequest.RefreshToken);
                if (dbRefreshToken == null)                    
                return new ApiResponse<AuthResultDto>(false, "RefreshToken does not exist in our DB", 400, new List<string>());

                //Check 5 : Validate Id
                var jti = tokenInVerification.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Jti).Value;
                if (dbRefreshToken.JwtId != jti)
                    return new ApiResponse<AuthResultDto>(false, "Token does not match", 400, new List<string>());               

                //Check 6 : RefreshToken expiration
                if (dbRefreshToken.DateExpire <= DateTime.UtcNow)
                    return new ApiResponse<AuthResultDto>(false, "Your refresh token has expired, please re-authenticate", 400, new List<string>());

                //Check 7: Refesh token Revoked
                if (dbRefreshToken.IsRevoked)
                    return new ApiResponse<AuthResultDto>(false, "Refresh token is revoked", 400, new List<string>());

                //Generate new token wiht existing refresh token
                var dbUserData = await _userManager.FindByIdAsync(dbRefreshToken.UserId);
                var newTokenResponse = await GenerateJwtTokenAsync(dbUserData, tokenRequest.RefreshToken);
                //return await newTokenResponse;
                return new ApiResponse<AuthResultDto>(true, "Refresh token generated success", 200,  newTokenResponse, new List<string>());

            }
            catch (SecurityTokenExpiredException)
            {
                // Handle the expired token exception
                return new ApiResponse<AuthResultDto>(false, "Token has expired", 401, new List<string>());
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return new ApiResponse<AuthResultDto>(false, $"An error occurred: {ex.Message}", 500, new List<string>());
            }


        }

        private DateTime UnixTimeStampToDateTimeInUTC(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp);
            return dateTimeVal;
        }
    }
}

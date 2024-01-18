using MyBook.Domain;
using MyBook.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ApiResponse<string>> Register(RegisterDto registerDto);
    }
}

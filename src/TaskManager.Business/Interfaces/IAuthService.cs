using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Business.DTOs.Auth;
using TaskManager.Business.DTOs.Users;

namespace TaskManager.Business.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);   
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
    }
}

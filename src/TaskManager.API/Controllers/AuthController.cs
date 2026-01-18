using Microsoft.AspNetCore.Mvc;
using TaskManager.Business.Common;
using TaskManager.Business.DTOs.Auth;
using TaskManager.Business.Interfaces;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            var response = new ApiResponse<AuthResponseDto>(result, "Login success");
            return Ok(response);
        }
    }
}

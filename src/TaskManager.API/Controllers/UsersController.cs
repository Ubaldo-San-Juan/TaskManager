using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Business.Common;
using TaskManager.Business.DTOs.Users;
using TaskManager.Business.Interfaces;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            var response = new ApiResponse<IEnumerable<UserDto>>(users, "List of users");
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new ApiResponse<UserDto>("User not found"));
            }
            var response = new ApiResponse<UserDto>(user, "User info");
            return Ok(response);
        }
        [HttpGet("email")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserByIdAsync(string email)
        {
            var user = await _userService.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound(new ApiResponse<UserDto>("User not found"));
            }
            var response = new ApiResponse<UserDto>(user, "User info");
            return Ok(response);
        }
    }
}

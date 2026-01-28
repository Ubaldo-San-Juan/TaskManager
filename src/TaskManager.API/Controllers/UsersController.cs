using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Business.Common;
using TaskManager.Business.DTOs.Users;
using TaskManager.Business.Interfaces;

namespace TaskManager.API.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            var response = new ApiResponse<IEnumerable<UserDto>>(users, "List of users");
            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserByIdAsync(int id)
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
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserByEmailAsync([FromQuery] string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new ApiResponse<UserDto>("User not found"));
            }
            var response = new ApiResponse<UserDto>(user, "User info");
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserDto>>> CreateUserAsync([FromBody] CreateUserDto createUserDto)
        {
            var userToCreate = await _userService.CreateUserAsync(createUserDto);
            var response = new ApiResponse<UserDto>(userToCreate, "User created successfully");

            return CreatedAtRoute("GetUserById", new { id = userToCreate.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateUserAsync([FromRoute] int id, [FromBody] UpdateUserDto updateUserDto)
        {
            var userUpdated = await _userService.UpdateUserAsync(id, updateUserDto);
            var response = new ApiResponse<UserDto>(userUpdated, "User updated successfully");
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteUserAsync([FromRoute] int id)
        {
            await _userService.DeleteUserAsync(id);
            var response = new ApiResponse<string>(null, "User deleted successfully");
            return Ok(response);
        }
    }
}

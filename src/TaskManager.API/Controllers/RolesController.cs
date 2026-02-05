using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Business.Common;
using TaskManager.Business.DTOs.Roles;
using TaskManager.Business.DTOs.Users;
using TaskManager.Business.Interfaces;


namespace TaskManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> GetAllRolesAsync()
        {
            var roles = await _roleService.GetAllRolesAsync();
            var response = new ApiResponse<IEnumerable<RoleDto>>(roles, "List of roles");
            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetRoleById")]
        public async Task<ActionResult<ApiResponse<RoleDto>>> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound(new ApiResponse<RoleDto>("Rol no found"));
            }
            var response = new ApiResponse<RoleDto>(role, "Role info");
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<RoleDto>>> CreateRoleAsync([FromBody] CreateRoleDto createRoleDto)
        {
            var roleToCreate = await _roleService.CreateRoleAsync(createRoleDto);
            var response = new ApiResponse<RoleDto>(roleToCreate, "Role create successfully");
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<RoleDto>>> UpdateRoleAsync([FromRoute] int id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            var roleUpdated = await _roleService.UpdateRoleAsync(id, updateRoleDto);
            var response = new ApiResponse<RoleDto>(roleUpdated, "Role updated successfully");
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteRoleAsync([FromRoute] int id)
        {
            await _roleService.DeleteRoleAsync(id);
            var response = new ApiResponse<string>(null, "Role deleted successfully");
            return Ok(response);
        }
    }
}

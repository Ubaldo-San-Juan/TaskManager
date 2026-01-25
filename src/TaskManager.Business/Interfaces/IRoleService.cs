
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Business.DTOs.Roles;

namespace TaskManager.Business.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int roleId);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto);
        Task UpdateRoleAsync(int roleId, UpdateRoleDto updateRoleDto);
        Task DeleteRoleAsync(int roleId);
    }
}

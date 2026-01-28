using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Business.DTOs.Roles;
using TaskManager.Business.Interfaces;
using TaskManager.Data.Entities;
using TaskManager.Data.Interfaces;

namespace TaskManager.Business.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            var existingRole = await _roleRepository.GetRoleByNameAsync(createRoleDto.Name);
            if (existingRole != null)
            {
                throw new InvalidOperationException($"Role with name '{createRoleDto.Name}' already exists.");
            }

            var roleEntity = _mapper.Map<Role>(createRoleDto);
            
            await _roleRepository.CreateRoleAsync(roleEntity);
            return _mapper.Map<RoleDto>(roleEntity);
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            var roleEntity = await _roleRepository.GetRoleByIdAsync(roleId);
            if (roleEntity == null)
            {
                throw new KeyNotFoundException($"Role with ID {roleId} not found.");
            }

            if (roleEntity.Name == "Admin" || roleEntity.Name == "User")
            {
                throw new InvalidOperationException("Cannot delete default roles 'Admin' or 'User'.");
            }

            roleEntity.IsDeleted = true;
            roleEntity.DeletedAt = DateTime.UtcNow;
            await _roleRepository.UpdateRoleAsync(roleEntity);
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRoleAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int roleId)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {roleId} not found.");
            }
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<RoleDto> UpdateRoleAsync(int roleId, UpdateRoleDto updateRoleDto)
        {
            var roleEntity = await _roleRepository.GetRoleByIdAsync(roleId);
            if (roleEntity == null)
            {
                throw new KeyNotFoundException($"Role with ID {roleId} not found.");
            }

            if (updateRoleDto.Name != roleEntity.Name)
            {
                var existingRole = await _roleRepository.GetRoleByNameAsync(updateRoleDto.Name);
                if (existingRole != null)
                {
                    throw new InvalidOperationException($"Role with name '{updateRoleDto.Name}' already exists.");
                }
            }

            _mapper.Map(updateRoleDto, roleEntity);
            roleEntity.UpdatedAt = DateTime.UtcNow;
            await _roleRepository.UpdateRoleAsync(roleEntity);
            return _mapper.Map<RoleDto>(roleEntity);
        }
    }
}

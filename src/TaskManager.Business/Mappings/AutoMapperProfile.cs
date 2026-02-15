using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Business.DTOs.Roles;
using TaskManager.Business.DTOs.Tasks;
using TaskManager.Business.DTOs.Users;
using TaskManager.Data.Entities;

namespace TaskManager.Business.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            // Mapping for User Entity and DTOs
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // Mapping for Role Entity and DTOs
            CreateMap<Role, RoleDto>();
            CreateMap<CreateRoleDto, Role>();
            CreateMap<UpdateRoleDto, Role>();

            // Mapping for TodoTask Entity and DTOs
            CreateMap<TodoTask, TodoTaskDto>();
            CreateMap<CreateTaskDto, TodoTask>();
            CreateMap<UpdateTaskDto, TodoTask>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}

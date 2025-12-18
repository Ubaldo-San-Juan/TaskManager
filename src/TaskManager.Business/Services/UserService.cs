using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Business.DTOs.Users;
using TaskManager.Business.Interfaces;
using TaskManager.Data.Interfaces;

namespace TaskManager.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserDto> _createValidator;

        public UserService(IUserRepository userRepository, IMapper mapper, IValidator<CreateUserDto> createValidator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _createValidator = createValidator;
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}

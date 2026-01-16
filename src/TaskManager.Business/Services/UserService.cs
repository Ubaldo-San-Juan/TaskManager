using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Business.DTOs.Users;
using TaskManager.Business.Interfaces;
using TaskManager.Data.Entities;
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

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createUserDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var userExist = await _userRepository.GetUserByEmailAsync(createUserDto.Email);
            if (userExist != null)
            {
                throw new InvalidOperationException("Ya existe un usuario registrado con este email.");
            }

            var userEntity = _mapper.Map<User>(createUserDto);
            userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
            await _userRepository.CreateUserAsync(userEntity);

            return _mapper.Map<UserDto>(userEntity);
        }
    }
}

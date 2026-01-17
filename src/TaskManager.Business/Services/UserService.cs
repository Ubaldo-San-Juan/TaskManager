using AutoMapper;
using FluentValidation;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IValidator<UpdateUserDto> _updateValidator;

        public UserService(
            IUserRepository userRepository, 
            IMapper mapper, 
            IValidator<CreateUserDto> createValidator, 
            IValidator<UpdateUserDto> updateValidator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
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
                throw new InvalidOperationException("An user is already exists with this email.");
            }

            var userEntity = _mapper.Map<User>(createUserDto);
            userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
            await _userRepository.CreateUserAsync(userEntity);

            return _mapper.Map<UserDto>(userEntity);
        }

        public async Task<UserDto> UpdateUserAsync(int idUser, UpdateUserDto updateUserDto)
        {
            //Validate user input
            var validationResult = _updateValidator.Validate(updateUserDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Check if user exists
            var existUserToUpdate = await _userRepository.GetUserByIdAsync(idUser);
            if (existUserToUpdate == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Verify if email is being updated to an email that already exists
            if(updateUserDto.Email != existUserToUpdate.Email)
            {
                var userWithEmail = await _userRepository.GetUserByEmailAsync(updateUserDto.Email);
                if (userWithEmail != null)
                {
                    throw new InvalidOperationException("An user is already exists with this email.");
                }
            }
            
            existUserToUpdate.UpdatedAt = DateTime.UtcNow;

            _mapper.Map(updateUserDto, existUserToUpdate);
            await _userRepository.UpdateUserAsync(existUserToUpdate);
            return _mapper.Map<UserDto>(existUserToUpdate);
        }
    }
}

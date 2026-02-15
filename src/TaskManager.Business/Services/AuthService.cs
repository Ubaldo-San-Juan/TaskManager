using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Business.Common;
using TaskManager.Business.DTOs.Auth;
using TaskManager.Business.DTOs.Users;
using TaskManager.Business.Interfaces;
using TaskManager.Data.Entities;
using TaskManager.Data.Interfaces;
using TaskManager.Data.Repositories;

namespace TaskManager.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public AuthService(
            IUserRepository userRepository,
            IOptions<JwtSettings> jwtSettings,
            IValidator<LoginDto> loginValidator,
            IValidator<RegisterDto> registerValidator,
            IRoleRepository roleRepository,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Validate the login DTO
            var validationResult = await _loginValidator.ValidateAsync(loginDto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Search for the user by email
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Verify the password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Generate JWT token
            string token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = token
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var validationResult = await _registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var userExist = await _userRepository.GetUserByEmailAsync(registerDto.Email);
            if (userExist != null)
            {
                throw new InvalidOperationException("An user is already exists with this email.");
            }

            var role = await _roleRepository.GetRoleByNameAsync("User");
            if (role == null)
            {
                throw new InvalidOperationException("Default role 'User' not found.");
            }

            var userEntity = _mapper.Map<User>(registerDto);
            userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            userEntity.RoleId = role.Id;
            userEntity.CreatedAt = DateTime.UtcNow;

            await _userRepository.CreateUserAsync(userEntity);
            return _mapper.Map<UserDto>(userEntity);
        }

        private string GenerateJwtToken(User user)
        {
            // Create claims based on user information
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject - user ID
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier for the token
                new Claim("Name", user.Name),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
            };

            // Create security key and signing credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Configure token properties
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = creds
            };

            // Create the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    }
}

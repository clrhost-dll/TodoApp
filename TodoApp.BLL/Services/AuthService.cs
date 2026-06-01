using Microsoft.Extensions.Logging;
using TodoApp.BLL.DTOs.Auth;
using TodoApp.BLL.Exceptions;
using TodoApp.BLL.Helpers;
using TodoApp.BLL.Interfaces;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories.Interfaces;

namespace TodoApp.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepository,
            JwtTokenGenerator jwtTokenGenerator,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            User? existingUser =
                await _userRepository.GetByEmailAsync(dto.Email);

            if (existingUser is not null)
            {
                _logger.LogWarning("Registration failed - user already exists: {Email}",dto.Email);
                throw new BadRequestException("User already exists");
            }

            User user = new()
            {
                Id = Guid.NewGuid(),
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _userRepository.CreateAsync(user);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation("User registered successfully: {Email}",dto.Email);

            string token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserName = user.UserName
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            User? user =
                await _userRepository.GetByEmailAsync(dto.Email);

            if (user is null)
            {
                _logger.LogWarning("Login failed - user not found: {Email}",dto.Email);
                throw new UnauthorizedException("Invalid credentials");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Login failed - invalid password for: {Email}",dto.Email);
                throw new UnauthorizedException("Invalid credentials");
            }

            _logger.LogInformation("User logged in successfully: {Email}",dto.Email);

            string token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserName = user.UserName
            };
        }
    }
}
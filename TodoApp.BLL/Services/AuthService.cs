using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.BLL.DTOs.Auth;
using TodoApp.BLL.Helpers;
using TodoApp.BLL.Interfaces;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories.Interfaces;
using TodoApp.BLL.Exceptions;

namespace TodoApp.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthService(
            IUserRepository userRepository,
            JwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            User? existingUser =
                await _userRepository.GetByEmailAsync(dto.Email);

            if (existingUser is not null)
            {
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

            string token =
                _jwtTokenGenerator.GenerateToken(user);

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
                throw new UnauthorizedException("Invalid credentials");
            }

            bool isPasswordValid =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new UnauthorizedException("Invalid credentials");
            }

            string token =
                _jwtTokenGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                UserName = user.UserName
            };
        }
    }
}

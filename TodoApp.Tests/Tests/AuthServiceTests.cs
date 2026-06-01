using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TodoApp.BLL.DTOs.Auth;
using TodoApp.BLL.Exceptions;
using TodoApp.BLL.Helpers;
using TodoApp.BLL.Services;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories.Interfaces;

namespace TodoApp.Tests.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Jwt:Key", TestConstants.JwtKey },
                    { "Jwt:Issuer", TestConstants.JwtIssuer },
                    { "Jwt:Audience", TestConstants.JwtAudience }
                })
                .Build();

            _jwtTokenGenerator = new JwtTokenGenerator(config);

            var logger = new Mock<ILogger<AuthService>>();

            _authService = new AuthService(
                _userRepositoryMock.Object,
                _jwtTokenGenerator,
                logger.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnToken_WhenUserDoesNotExist()
        {
            // Arrange
            var dto = new RegisterDto
            {
                UserName = "testuser",
                Email = "test@test.com",
                Password = "password123"
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);

            _userRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            _userRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _authService.RegisterAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
            result.UserName.Should().Be(dto.UserName);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrowBadRequestException_WhenUserAlreadyExists()
        {
            // Arrange
            var dto = new RegisterDto
            {
                UserName = "testuser",
                Email = "test@test.com",
                Password = "password123"
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync(new User { Email = dto.Email });

            // Act
            var act = async () => await _authService.RegisterAsync(dto);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("User already exists");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "test@test.com",
                Password = "password123"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "testuser",
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
            result.UserName.Should().Be(user.UserName);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowUnauthorizedException_WhenUserNotFound()
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "notexist@test.com",
                Password = "password123"
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);

            // Act
            var act = async () => await _authService.LoginAsync(dto);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid credentials");
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowUnauthorizedException_WhenPasswordIsInvalid()
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "test@test.com",
                Password = "wrongpassword"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "testuser",
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword")
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            // Act
            var act = async () => await _authService.LoginAsync(dto);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("Invalid credentials");
        }
    }
}
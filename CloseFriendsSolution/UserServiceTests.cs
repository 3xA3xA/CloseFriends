using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using CloseFriends.Application.Services;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;
using CloseFriends.Domain.Entities;

namespace CloseFriends.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task RegisterUserAsync_ValidInput_ReturnsUserDto()
        {
            // Arrange
            var userRepoMock = new Mock<IUserRepository>();

            // Пользователя с таким email нет
            userRepoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            userRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var service = new UserService(userRepoMock.Object);

            var dto = new UserRegistrationDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "Password123!"
            };

            // Act
            var result = await service.RegisterUserAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.Email, result.Email);
        }

        [Fact]
        public async Task RegisterUserAsync_EmailAlreadyExists_ThrowsArgumentException()
        {
            // Arrange
            var userRepoMock = new Mock<IUserRepository>();
            // Если пользователь с таким email уже существует
            userRepoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            var service = new UserService(userRepoMock.Object);

            var dto = new UserRegistrationDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "Password123!"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.RegisterUserAsync(dto));
        }
    }
}

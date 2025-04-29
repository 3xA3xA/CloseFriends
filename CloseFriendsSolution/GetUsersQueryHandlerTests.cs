using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using CloseFriends.Application.Queries;
using CloseFriends.Application.Interfaces;
using CloseFriends.Domain.Entities;
using CloseFriends.Application.DTOs;

namespace CloseFriends.Tests
{
    public class GetUsersQueryHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ReturnsUserDtos()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
                new User { Id = 2, Name = "Bob", Email = "bob@example.com" }
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var handler = new GetUsersQueryHandler(userRepositoryMock.Object);

            // Act
            IEnumerable<UserDto> result = await handler.HandleAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, u => u.Name == "Alice" && u.Email == "alice@example.com");
        }
    }
}

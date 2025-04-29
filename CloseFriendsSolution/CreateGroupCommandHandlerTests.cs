using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Commands;
using CloseFriends.Application.Interfaces;
using CloseFriends.Domain.Entities;

namespace CloseFriends.Tests
{
    public class CreateGroupCommandHandlerTests
    {
        [Fact]
        public async Task HandleAsync_InvalidName_ThrowsArgumentException()
        {
            // Arrange
            var groupRepoMock = new Mock<IGroupRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var handler = new CreateGroupCommandHandler(groupRepoMock.Object, userRepoMock.Object);

            var dto = new GroupCreationDto
            {
                Name = "", // пустое им€ Ц недопустимо
                Type = GroupType.Family,
                PhotoUrl = "http://example.com/image.png",
                Description = "Test description",
                OwnerId = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(dto));
        }

        [Fact]
        public async Task HandleAsync_InvalidOwner_ThrowsArgumentException()
        {
            // Arrange
            var groupRepoMock = new Mock<IGroupRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.ExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var handler = new CreateGroupCommandHandler(groupRepoMock.Object, userRepoMock.Object);

            var dto = new GroupCreationDto
            {
                Name = "Test Group",
                Type = GroupType.Family,
                PhotoUrl = "http://example.com/image.png",
                Description = "Test description",
                OwnerId = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(dto));
        }

        [Fact]
        public async Task HandleAsync_ValidInput_ReturnsGroupDto()
        {
            // Arrange
            var groupRepoMock = new Mock<IGroupRepository>();
            var userRepoMock = new Mock<IUserRepository>();

            userRepoMock.Setup(r => r.ExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            groupRepoMock.Setup(r => r.AddAsync(It.IsAny<Group>())).Returns(Task.CompletedTask);
            groupRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var handler = new CreateGroupCommandHandler(groupRepoMock.Object, userRepoMock.Object);

            var dto = new GroupCreationDto
            {
                Name = "Valid Group",
                Type = GroupType.Family,
                PhotoUrl = "http://example.com/image.png",
                Description = "A valid test group",
                OwnerId = 1
            };

            // Act
            GroupDto result = await handler.HandleAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.Type.ToString(), result.Type);
            Assert.Equal(dto.OwnerId, result.OwnerId);
            Assert.False(string.IsNullOrEmpty(result.ShareLink));

            groupRepoMock.Verify(r => r.AddAsync(It.IsAny<Group>()), Times.Once);
            groupRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}

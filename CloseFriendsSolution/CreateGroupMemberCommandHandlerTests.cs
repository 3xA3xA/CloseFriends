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
    public class CreateGroupMemberCommandHandlerTests
    {
        [Fact]
        public async Task HandleAsync_GroupNotFound_ThrowsArgumentException()
        {
            // Arrange
            var groupMemberRepoMock = new Mock<IGroupMemberRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var groupRepoMock = new Mock<IGroupRepository>();

            groupRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Group)null);

            var handler = new CreateGroupMemberCommandHandler(
                groupMemberRepoMock.Object, userRepoMock.Object, groupRepoMock.Object);

            var dto = new GroupMemberCreationDto
            {
                GroupId = 1,
                UserId = 1,
                Role = "Member"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(dto));
        }

        [Fact]
        public async Task HandleAsync_UserNotFound_ThrowsArgumentException()
        {
            // Arrange
            var groupMemberRepoMock = new Mock<IGroupMemberRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var groupRepoMock = new Mock<IGroupRepository>();

            groupRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Group { Id = 1 });
            userRepoMock.Setup(r => r.ExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            var handler = new CreateGroupMemberCommandHandler(
                groupMemberRepoMock.Object, userRepoMock.Object, groupRepoMock.Object);

            var dto = new GroupMemberCreationDto
            {
                GroupId = 1,
                UserId = 1,
                Role = "Member"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(dto));
        }

        [Fact]
        public async Task HandleAsync_AlreadyMember_ThrowsArgumentException()
        {
            // Arrange
            var groupMemberRepoMock = new Mock<IGroupMemberRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var groupRepoMock = new Mock<IGroupRepository>();

            groupRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Group { Id = 1 });
            userRepoMock.Setup(r => r.ExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            groupMemberRepoMock.Setup(r => r.IsMemberAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var handler = new CreateGroupMemberCommandHandler(
                groupMemberRepoMock.Object, userRepoMock.Object, groupRepoMock.Object);

            var dto = new GroupMemberCreationDto
            {
                GroupId = 1,
                UserId = 1,
                Role = "Admin"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(dto));
        }

        [Fact]
        public async Task HandleAsync_ValidInput_ReturnsGroupMemberDto()
        {
            // Arrange
            var groupMemberRepoMock = new Mock<IGroupMemberRepository>();
            var userRepoMock = new Mock<IUserRepository>();
            var groupRepoMock = new Mock<IGroupRepository>();

            groupRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Group { Id = 1 });
            userRepoMock.Setup(r => r.ExistsByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            groupMemberRepoMock.Setup(r => r.IsMemberAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);
            groupMemberRepoMock.Setup(r => r.AddAsync(It.IsAny<GroupMember>())).Returns(Task.CompletedTask);
            groupMemberRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var handler = new CreateGroupMemberCommandHandler(
                groupMemberRepoMock.Object, userRepoMock.Object, groupRepoMock.Object);

            var dto = new GroupMemberCreationDto
            {
                GroupId = 1,
                UserId = 1,
                Role = "" // проверка дефолтного значения "Member"
            };

            // Act
            GroupMemberDto result = await handler.HandleAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.GroupId, result.GroupId);
            Assert.Equal(dto.UserId, result.UserId);
            Assert.Equal("Member", result.Role);  // mặc định должна ставиться "Member"
            groupMemberRepoMock.Verify(r => r.AddAsync(It.IsAny<GroupMember>()), Times.Once);
            groupMemberRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}

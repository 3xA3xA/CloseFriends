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
    public class GetGroupsQueryHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ReturnsGroupDtos()
        {
            // Arrange
            var groups = new List<Group>
            {
                new Group { Id = 1, Name = "Group1", Type = GroupType.Family, OwnerId = 1, PhotoUrl = "url1", Description = "desc1", ShareLink = "link1", CreatedAt = System.DateTime.UtcNow },
                new Group { Id = 2, Name = "Group2", Type = GroupType.Friends, OwnerId = 2, PhotoUrl = "url2", Description = "desc2", ShareLink = "link2", CreatedAt = System.DateTime.UtcNow }
            };

            var groupRepositoryMock = new Mock<IGroupRepository>();
            groupRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(groups);

            var handler = new GetGroupsQueryHandler(groupRepositoryMock.Object);

            // Act
            IEnumerable<GroupDto> result = await handler.HandleAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, g => g.Name == "Group1" && g.Type == GroupType.Family.ToString());
        }
    }
}

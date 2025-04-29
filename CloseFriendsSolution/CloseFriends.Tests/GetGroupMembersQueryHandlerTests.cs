using System;
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
    public class GetGroupMembersQueryHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ReturnsGroupMemberDtos()
        {
            // Arrange
            var members = new List<GroupMember>
            {
                new GroupMember { Id = 1, GroupId = 1, UserId = 1, Role = "Member", JoinedAt = DateTime.UtcNow },
                new GroupMember { Id = 2, GroupId = 1, UserId = 2, Role = "Admin", JoinedAt = DateTime.UtcNow }
            };

            var memberRepositoryMock = new Mock<IGroupMemberRepository>();
            memberRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(members);

            var handler = new GetGroupMembersQueryHandler(memberRepositoryMock.Object);

            // Act
            IEnumerable<GroupMemberDto> result = await handler.HandleAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, m => m.UserId == 1 && m.Role == "Member");
        }
    }
}

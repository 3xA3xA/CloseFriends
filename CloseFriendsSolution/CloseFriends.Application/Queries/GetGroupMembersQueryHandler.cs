using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;

namespace CloseFriends.Application.Queries
{
    public class GetGroupMembersQueryHandler : IGetGroupMembersQueryHandler
    {
        private readonly IGroupMemberRepository _groupMemberRepository;

        public GetGroupMembersQueryHandler(IGroupMemberRepository groupMemberRepository)
        {
            _groupMemberRepository = groupMemberRepository;
        }

        public async Task<IEnumerable<GroupMemberDto>> HandleAsync()
        {
            var members = await _groupMemberRepository.GetAllAsync();
            return members.Select(m => new GroupMemberDto
            {
                Id = m.Id,
                GroupId = m.GroupId,
                UserId = m.UserId,
                Role = m.Role,
                JoinedAt = m.JoinedAt
            });
        }
    }
}

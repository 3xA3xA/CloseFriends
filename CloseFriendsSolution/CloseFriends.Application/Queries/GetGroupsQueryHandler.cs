using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;

namespace CloseFriends.Application.Queries
{
    public class GetGroupsQueryHandler : IGetGroupsQueryHandler
    {
        private readonly IGroupRepository _groupRepository;

        public GetGroupsQueryHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<IEnumerable<GroupDto>> HandleAsync()
        {
            var groups = await _groupRepository.GetAllAsync();
            return groups.Select(g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Type = g.Type.ToString(),
                OwnerId = g.OwnerId,
                PhotoUrl = g.PhotoUrl,
                Description = g.Description,
                ShareLink = g.ShareLink,
                CreatedAt = g.CreatedAt
            });
        }
    }
}

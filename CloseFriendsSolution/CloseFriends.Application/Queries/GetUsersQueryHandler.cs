using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;

namespace CloseFriends.Application.Queries
{
    public class GetUsersQueryHandler : IGetUsersQueryHandler
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> HandleAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            });
        }
    }
}

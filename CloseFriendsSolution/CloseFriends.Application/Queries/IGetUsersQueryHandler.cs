using System.Collections.Generic;
using System.Threading.Tasks;
using CloseFriends.Application.DTOs;

namespace CloseFriends.Application.Queries
{
    public interface IGetUsersQueryHandler
    {
        Task<IEnumerable<UserDto>> HandleAsync();
    }
}

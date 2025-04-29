using System.Threading.Tasks;
using CloseFriends.Application.DTOs;

namespace CloseFriends.Application.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для управления участниками групп.
    /// </summary>
    public interface IGroupMemberService
    {
        /// <summary>
        /// Добавляет нового участника в группу.
        /// </summary>
        /// <param name="dto">DTO с данными для добавления участника.</param>
        /// <returns>Возвращает DTO добавленного участника.</returns>
        Task<GroupMemberDto> AddMemberAsync(GroupMemberCreationDto dto);
    }
}

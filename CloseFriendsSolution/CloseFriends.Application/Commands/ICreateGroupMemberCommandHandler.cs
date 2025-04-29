using System.Threading.Tasks;
using CloseFriends.Application.DTOs;

namespace CloseFriends.Application.Commands
{
    /// <summary>
    /// Интерфейс обработчика команды создания участника группы.
    /// </summary>
    public interface ICreateGroupMemberCommandHandler
    {
        /// <summary>
        /// Обрабатывает команду создания участника группы.
        /// </summary>
        /// <param name="dto">DTO с данными для добавления участника в группу.</param>
        /// <returns>Возвращает DTO созданного участника группы.</returns>
        Task<GroupMemberDto> HandleAsync(GroupMemberCreationDto dto);
    }
}

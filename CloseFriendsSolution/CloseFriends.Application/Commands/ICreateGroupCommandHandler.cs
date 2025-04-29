using System.Threading.Tasks;
using CloseFriends.Application.DTOs;

namespace CloseFriends.Application.Commands
{
    /// <summary>
    /// Интерфейс обработчика команды создания группы.
    /// </summary>
    public interface ICreateGroupCommandHandler
    {
        /// <summary>
        /// Обрабатывает команду создания группы.
        /// </summary>
        /// <param name="dto">DTO с данными для создания группы.</param>
        /// <returns>Возвращает DTO созданной группы.</returns>
        Task<GroupDto> HandleAsync(GroupCreationDto dto);
    }
}

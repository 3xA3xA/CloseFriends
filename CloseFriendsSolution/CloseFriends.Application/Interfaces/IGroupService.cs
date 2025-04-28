using System.Threading.Tasks;
using CloseFriends.Application.DTOs;

namespace CloseFriends.Application.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для управления группами.
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// Создаёт новую группу на основе предоставленных данных.
        /// </summary>
        /// <param name="dto">DTO с данными для создания группы.</param>
        /// <returns>Возвращает DTO созданной группы.</returns>
        Task<GroupDto> CreateGroupAsync(GroupCreationDto dto);
    }
}

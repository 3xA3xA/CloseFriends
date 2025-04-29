using System.Threading.Tasks;
using CloseFriends.Domain.Entities;

namespace CloseFriends.Application.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с данными участников групп.
    /// </summary>
    public interface IGroupMemberRepository
    {
        /// <summary>
        /// Добавляет нового участника группы в хранилище.
        /// </summary>
        Task AddAsync(GroupMember member);

        /// <summary>
        /// Проверяет, является ли пользователь уже участником указанной группы.
        /// </summary>
        Task<bool> IsMemberAsync(int groupId, int userId);

        /// <summary>
        /// Сохраняет изменения в хранилище данных.
        /// </summary>
        Task SaveChangesAsync();
    }
}

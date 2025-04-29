using System.Threading.Tasks;
using CloseFriends.Domain.Entities;

namespace CloseFriends.Application.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с данными группы.
    /// </summary>
    public interface IGroupRepository
    {
        /// <summary>
        /// Добавляет новую группу.
        /// </summary>
        Task AddAsync(Group group);

        /// <summary>
        /// Сохраняет изменения в хранилище данных.
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// Получает группу по её идентификатору.
        /// </summary>
        Task<Group> GetByIdAsync(int groupId);

        /// <summary>
        /// Получает все группы.
        /// </summary>
        Task<IEnumerable<Group>> GetAllAsync();
    }
}

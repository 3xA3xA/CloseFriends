using System.Threading.Tasks;
using CloseFriends.Domain.Entities;

namespace CloseFriends.Application.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для доступа к данным пользователя.
    /// Определяет контракт для операций с базой данных в отношении сущности User.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Проверяет, существует ли пользователь с указанным email.
        /// </summary>
        Task<bool> EmailExistsAsync(string email);

        /// <summary>
        /// Возвращает пользователя по указанному email.
        /// </summary>
        /// <param name="email">Email пользователя.</param>
        /// <returns>Пользователь, если найден, иначе null.</returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Проверяет, существует ли пользователь с указанным идентификатором.
        /// </summary>
        Task<bool> ExistsByIdAsync(int userId);

        /// <summary>
        /// Добавляет нового пользователя в хранилище данных.
        /// </summary>
        Task AddAsync(User user);

        /// <summary>
        /// Получает всех пользователей.
        /// </summary>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Сохраняет изменения в хранилище данных.
        /// </summary>
        Task SaveChangesAsync();
    }
}

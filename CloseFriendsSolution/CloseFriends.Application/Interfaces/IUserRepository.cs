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
        /// Добавляет нового пользователя в хранилище данных.
        /// </summary>
        Task AddAsync(User user);

        /// <summary>
        /// Сохраняет изменения в хранилище данных.
        /// </summary>
        Task SaveChangesAsync();
    }
}

using CloseFriends.Application.DTOs;
using CloseFriends.Domain.Entities;

namespace CloseFriends.Application.Interfaces
{
    /// <summary>
    /// Сервис для генерации JWT токенов.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Генерирует JWT токен для указанного пользователя.
        /// </summary>
        /// <param name="user">Пользователь, для которого генерируется токен.</param>
        /// <returns>DTO с токеном и временем его истечения.</returns>
        LoginResponseDto GenerateToken(User user);
    }
}

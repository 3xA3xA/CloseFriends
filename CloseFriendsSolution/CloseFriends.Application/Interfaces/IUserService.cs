using CloseFriends.Application.DTOs;
using CloseFriends.Domain.Entities;

namespace CloseFriends.Application.Interfaces
{
    /// <summary>
    /// Определяет контракт для бизнес-логики управления пользователями.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Регистрирует нового пользователя на основе данных регистрации.
        /// </summary>
        /// <param name="registrationDto">Данные для регистрации пользователя.</param>
        /// <returns>Возвращает DTO с основными данными зарегистрированного пользователя.</returns>
        Task<UserDto> RegisterUserAsync(UserRegistrationDto registrationDto);

        /// <summary>
        /// Проверяет учетные данные пользователя и возвращает его, если они корректны.
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>Сущность пользователя, если данные корректны, иначе null.</returns>
        Task<User> ValidateUserAsync(string email, string password);
    }
}

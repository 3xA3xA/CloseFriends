using CloseFriends.Application.DTOs;

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
    }
}

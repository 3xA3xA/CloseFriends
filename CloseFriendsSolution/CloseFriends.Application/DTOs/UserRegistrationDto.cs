namespace CloseFriends.Application.DTOs
{
    /// <summary>
    /// DTO для регистрации пользователя. Содержит данные, необходимые для создания новой учетной записи.
    /// </summary>
    public class UserRegistrationDto
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Электронная почта пользователя.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя в открытом виде (на входе, будет захэширован для безопасности).
        /// </summary>
        public string Password { get; set; }
    }
}

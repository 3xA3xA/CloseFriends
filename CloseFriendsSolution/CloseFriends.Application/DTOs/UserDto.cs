namespace CloseFriends.Application.DTOs
{
    /// <summary>
    /// DTO для возврата данных о пользователе после успешной регистрации.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Уникальный идентификатор пользователя.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Электронная почта пользователя.
        /// </summary>
        public string Email { get; set; }
    }
}

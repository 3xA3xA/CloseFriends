using System.ComponentModel.DataAnnotations;

namespace CloseFriends.Application.DTOs
{
    /// <summary>
    /// DTO для регистрации пользователя. Содержит данные, необходимые для создания новой учетной записи.
    /// </summary>
    public class UserRegistrationDto
    {
        /// <summary>
        /// Имя пользователя. Обязательно для заполнения.
        /// </summary>
        [Required(ErrorMessage = "Имя обязательно.")]
        public string Name { get; set; }

        /// <summary>
        /// Email пользователя. Обязателен и должен иметь корректный формат.
        /// </summary>
        [Required(ErrorMessage = "Email обязателен.")]
        [EmailAddress(ErrorMessage = "Неверный формат email.")]
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя. Обязателен и должен быть не менее 8 символов.
        /// </summary>
        [Required(ErrorMessage = "Пароль обязателен.")]
        [MinLength(8, ErrorMessage = "Пароль должен содержать не менее 8 символов.")]
        // При необходимости можно добавить [RegularExpression(...)] для проверки сложности.
        public string Password { get; set; }
    }
}

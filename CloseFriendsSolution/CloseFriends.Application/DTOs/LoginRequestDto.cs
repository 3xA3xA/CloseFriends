using System.ComponentModel.DataAnnotations;

namespace CloseFriends.Application.DTOs
{
    /// <summary>
    /// DTO для логина пользователя.
    /// Содержит email и пароль.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Email пользователя. Обязателен и должен иметь корректный формат.
        /// </summary>
        [Required(ErrorMessage = "Email обязателен.")]
        [EmailAddress(ErrorMessage = "Неверный формат email.")]
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя. Обязателен.
        /// </summary>
        [Required(ErrorMessage = "Пароль обязателен.")]
        public string Password { get; set; }
    }
}

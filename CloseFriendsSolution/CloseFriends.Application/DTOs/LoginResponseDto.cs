using System;

namespace CloseFriends.Application.DTOs
{
    /// <summary>
    /// DTO ответа логина с JWT токеном и временем его истечения.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// JWT токен.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Время, когда токен перестанет быть действительным.
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}

namespace CloseFriends.Application.Settings
{
    /// <summary>
    /// Настройки для генерации JWT токенов.
    /// Значения загружаются из конфигурации (appsettings.json или переменных окружения).
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Секретный ключ для шифрования токена.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Издатель токена.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Аудитория токена.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Время жизни токена в минутах.
        /// </summary>
        public int ExpiresInMinutes { get; set; }
    }
}

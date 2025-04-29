using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;
using CloseFriends.Application.Settings;
using CloseFriends.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CloseFriends.Application.Services
{
    /// <summary>
    /// Реализация сервиса для генерации JWT токенов.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Конструктор с внедрением настроек JWT.
        /// </summary>
        /// <param name="jwtSettings">Настройки для генерации JWT токенов.</param>
        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Генерирует JWT токен для указанного пользователя.
        /// </summary>
        /// <param name="user">Пользователь, для которого создается токен.</param>
        /// <returns>DTO с токеном и временем его истечения.</returns>
        public LoginResponseDto GenerateToken(User user)
        {
            // Формируем claims, содержащие основные сведения о пользователе
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim("name", user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Определяем ключ и алгоритм для подписи
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Рассчитываем время истечения токена
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes);

            // Создаем токен
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires
            };
        }
    }
}

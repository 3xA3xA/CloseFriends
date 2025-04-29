using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Xunit;
using Microsoft.Extensions.Options;
using CloseFriends.Application.Services;
using CloseFriends.Application.Settings;
using CloseFriends.Domain.Entities;
using CloseFriends.Application.DTOs;

namespace CloseFriends.Tests
{
    /// <summary>
    /// Тесты для TokenService.
    /// </summary>
    public class TokenServiceTests
    {
        /// <summary>
        /// Проверяет, что для корректного пользователя генерируется валидный JWT токен.
        /// </summary>
        [Fact]
        public void GenerateToken_ValidUser_ReturnsValidJwt()
        {
            // Arrange
            var jwtSettings = Options.Create(new JwtSettings
            {
                Key = "VerySecretKeyThatShouldBeLongEnoughForHmacSha256",
                Issuer = "CloseFriendsApi",
                Audience = "CloseFriendsClients",
                ExpiresInMinutes = 60
            });
            var tokenService = new TokenService(jwtSettings);
            var user = new User { Id = 1, Name = "Test User", Email = "test@example.com" };

            // Act
            LoginResponseDto tokenResponse = tokenService.GenerateToken(user);

            // Assert
            Assert.NotNull(tokenResponse);
            Assert.False(string.IsNullOrEmpty(tokenResponse.Token));
            Assert.True(tokenResponse.ExpiresAt > DateTime.UtcNow);

            // Разбираем токен для проверки claims
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenResponse.Token);
            var emailClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            Assert.Equal(user.Email, emailClaim);
        }
    }
}

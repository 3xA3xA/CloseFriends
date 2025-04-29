using Microsoft.AspNetCore.Mvc;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CloseFriends.Api.Controllers
{
    /// <summary>
    /// Контроллер для аутентификации и авторизации пользователей.
    /// Реализует endpoint для логина, генерирующий JWT токен.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Конструктор с внедрением зависимостей.
        /// </summary>
        public AuthController(IUserService userService, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// POST: api/auth/login
        /// Прием учетных данных пользователя, валидация и генерация JWT токена.
        /// </summary>
        /// <param name="loginDto">DTO с email и паролем.</param>
        /// <returns>JWT токен и время его истечения.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Неверные входные данные логина: {@LoginDto}", loginDto);
                return BadRequest(ModelState);
            }

            try
            {
                // Проверка учетных данных через IUserService.
                var user = await _userService.ValidateUserAsync(loginDto.Email, loginDto.Password);
                if (user == null)
                {
                    _logger.LogWarning("Неудачная попытка входа для email: {Email}", loginDto.Email);
                    return Unauthorized("Неверный email или пароль.");
                }

                var tokenResponse = _tokenService.GenerateToken(user);
                return Ok(tokenResponse);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Ошибка при логине для email: {Email}", loginDto.Email);
                throw;
            }
        }
    }
}

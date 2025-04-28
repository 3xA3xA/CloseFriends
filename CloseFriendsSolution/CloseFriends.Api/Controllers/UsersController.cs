using Microsoft.AspNetCore.Mvc;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;

namespace CloseFriends.Api.Controllers
{
    /// <summary>
    /// Контроллер для управления пользователями.
    /// Реализует endpoint для регистрации.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// Конструктор контроллера с внедрением зависимости IUserService.
        /// </summary>
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint для регистрации нового пользователя.
        /// Принимает данные регистрации и возвращает данные зарегистрированного пользователя.
        /// </summary>
        /// <param name="registrationDto">DTO с данными регистрации.</param>
        /// <returns>DTO с данными нового пользователя.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationDto)
        {
            // Валидируем входные данные
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newUser = await _userService.RegisterUserAsync(registrationDto);
                return Ok(newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя");
                return BadRequest(ex.Message);
            }
        }
    }
}

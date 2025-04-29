using Microsoft.AspNetCore.Mvc;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;
using CloseFriends.Application.Queries;
using Swashbuckle.AspNetCore.Annotations;

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
        /// Регистрирует нового пользователя.
        /// </summary>
        [HttpPost("register")]
        [SwaggerOperation(
            Summary = "Регистрация нового пользователя",
            Description = "Принимает данные регистрации и создает нового пользователя. Возвращает данные созданного пользователя."
        )]
        [SwaggerResponse(200, "Пользователь успешно зарегистрирован", typeof(UserDto))]
        [SwaggerResponse(400, "Неверные входные данные")]
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

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Получение всех пользователей",
            Description = "Возвращает список всех зарегистрированных пользователей."
        )]
        [SwaggerResponse(200, "Список пользователей успешно получен", typeof(IEnumerable<UserDto>))]
        public async Task<IActionResult> GetAllUsers([FromServices] IGetUsersQueryHandler queryHandler)
        {
            try
            {
                var users = await queryHandler.HandleAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка пользователей");
                return StatusCode(500, "Ошибка сервера");
            }
        }
    }
}

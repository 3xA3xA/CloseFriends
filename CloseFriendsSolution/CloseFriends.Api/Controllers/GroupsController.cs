using Microsoft.AspNetCore.Mvc;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;
using CloseFriends.Application.Queries;
using CloseFriends.Application.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace CloseFriends.Api.Controllers
{
    /// <summary>
    /// Контроллер для управления группами.
    /// Делегирует логику создания группы слою Application.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly ILogger<GroupsController> _logger;

        /// <summary>
        /// Конструктор с внедрением зависимости IGroupService.
        /// </summary>
        public GroupsController(IGroupService groupService, ILogger<GroupsController> logger)
        {
            _groupService = groupService;
            _logger = logger;
        }

        /// <summary>
        /// Создает новую группу.
        /// </summary>
        /// <param name="dto">Данные для создания группы.</param>
        /// <param name="commandHandler">Обработчик команды создания группы.</param>
        /// <returns>HTTP-ответ с DTO созданной группы.</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Создание новой группы",
            Description = "Принимает данные группы, валидирует их и создает новую группу. Возвращает созданный объект с уникальным идентификатором."
        )]
        [SwaggerResponse(201, "Группа успешно создана", typeof(GroupDto))]
        [SwaggerResponse(400, "Неверные входные данные")]
        public async Task<IActionResult> CreateGroup(
            [FromBody] GroupCreationDto dto,
            [FromServices] ICreateGroupCommandHandler commandHandler)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                GroupDto createdGroup = await commandHandler.HandleAsync(dto);
                return CreatedAtAction(nameof(GetAllGroups), new { id = createdGroup.Id }, createdGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании группы");
                return BadRequest(ex.Message);
            }
        }

        // Пример GET-метода для CreatedAtAction
        [HttpGet]
        [SwaggerOperation(
            Summary = "Получение списка групп",
            Description = "Возвращает список всех групп."
        )]
        [SwaggerResponse(200, "Список успешно получен", typeof(IEnumerable<GroupDto>))]
        public async Task<IActionResult> GetAllGroups([FromServices] IGetGroupsQueryHandler queryHandler)
        {
            try
            {
                var groups = await queryHandler.HandleAsync();
                return Ok(groups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка групп");
                return StatusCode(500, "Ошибка сервера");
            }
        }
    }
}

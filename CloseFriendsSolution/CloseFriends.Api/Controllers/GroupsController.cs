using Microsoft.AspNetCore.Mvc;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;

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
        /// Endpoint для создания новой группы.
        /// Принимает DTO с данными группы и возвращает созданный объект.
        /// </summary>
        /// <param name="dto">Данные для создания группы.</param>
        /// <returns>HTTP-ответ с DTO созданной группы.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] GroupCreationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdGroup = await _groupService.CreateGroupAsync(dto);
                return Ok(createdGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании группы");
                return BadRequest(ex.Message);
            }
        }
    }
}

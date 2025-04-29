using Microsoft.AspNetCore.Mvc;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;
using System;
using System.Threading.Tasks;
using CloseFriends.Application.Queries;
using CloseFriends.Application.Commands;
using Swashbuckle.AspNetCore.Annotations;

namespace CloseFriends.Api.Controllers
{
    /// <summary>
    /// Контроллер для управления участниками групп.
    /// Делегирует добавление нового участника в сервис управления участниками.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GroupMembersController : ControllerBase
    {
        private readonly IGroupMemberService _groupMemberService;
        private readonly ILogger<GroupMembersController> _logger;

        public GroupMembersController(IGroupMemberService groupMemberService, ILogger<GroupMembersController> logger)
        {
            _groupMemberService = groupMemberService;
            _logger = logger;
        }

        /// <summary>
        /// Добавляет нового участника в группу.
        /// </summary>
        /// <param name="dto">Данные для добавления участника.</param>
        /// <param name="commandHandler">Обработчик команды создания участника группы.</param>
        /// <returns>HTTP-ответ с DTO созданного участника группы.</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Добавление нового участника в группу",
            Description = "Принимает данные для добавления участника, проверяет существование группы и пользователя, а также уникальность участника. Возвращает созданный объект участника."
        )]
        [SwaggerResponse(201, "Участник успешно добавлен", typeof(GroupMemberDto))]
        [SwaggerResponse(400, "Неверные входные данные или участник уже существует")]
        public async Task<IActionResult> CreateGroupMember(
            [FromBody] GroupMemberCreationDto dto,
            [FromServices] ICreateGroupMemberCommandHandler commandHandler)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                GroupMemberDto createdMember = await commandHandler.HandleAsync(dto);
                return CreatedAtAction(nameof(GetAllGroupMembers), new { id = createdMember.Id }, createdMember);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении участника в группу");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получает список всех участников групп.
        /// </summary>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Получение всех участников групп",
            Description = "Возвращает список всех участников групп."
        )]
        [SwaggerResponse(200, "Список успешно получен", typeof(IEnumerable<GroupMemberDto>))]
        public async Task<IActionResult> GetAllGroupMembers([FromServices] IGetGroupMembersQueryHandler queryHandler)
        {
            try
            {
                var members = await queryHandler.HandleAsync();
                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении участников групп");
                return StatusCode(500, "Ошибка сервера");
            }
        }
    }
}

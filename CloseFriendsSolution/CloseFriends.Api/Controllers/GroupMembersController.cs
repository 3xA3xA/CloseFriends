using Microsoft.AspNetCore.Mvc;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;
using System;
using System.Threading.Tasks;

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
        [HttpPost]
        public async Task<IActionResult> AddMember([FromBody] GroupMemberCreationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var member = await _groupMemberService.AddMemberAsync(dto);
                return Ok(member);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении участника в группу");
                return BadRequest(ex.Message);
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CloseFriends.Application.DTOs
{
    /// <summary>
    /// DTO для добавления участника группы.
    /// </summary>
    public class GroupMemberCreationDto
    {
        /// <summary>
        /// Идентификатор группы, в которую добавляется участник.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "GroupId должен быть положительным числом.")]
        public int GroupId { get; set; }

        /// <summary>
        /// Идентификатор пользователя, который добавляется в группу.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "UserId должен быть положительным числом.")]
        public int UserId { get; set; }

        /// <summary>
        /// Роль участника в группе (например, "Admin" или "Member"). Если не указано, бизнес-логика установит значение по умолчанию.
        /// </summary>
        public string Role { get; set; }
    }
}
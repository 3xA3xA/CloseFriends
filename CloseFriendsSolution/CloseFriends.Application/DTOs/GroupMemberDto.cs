using System;

namespace CloseFriends.Application.DTOs
{
    /// <summary>
    /// DTO, представляющий участника группы.
    /// </summary>
    public class GroupMemberDto
    {
        /// <summary>
        /// Уникальный идентификатор записи участника группы.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор группы.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Роль участника в группе.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Дата присоединения к группе.
        /// </summary>
        public DateTime JoinedAt { get; set; }
    }
}

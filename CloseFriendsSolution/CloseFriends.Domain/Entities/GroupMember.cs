using System;

namespace CloseFriends.Domain.Entities
{
    /// <summary>
    /// Представляет участника группы.
    /// </summary>
    public class GroupMember
    {
        /// <summary>
        /// Уникальный идентификатор записи участия.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя, который является участником.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Идентификатор группы, в которой состоит пользователь.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Дата присоединения пользователя к группе.
        /// </summary>
        public DateTime JoinedAt { get; set; }

        /// <summary>
        /// Роль участника в группе, например "администратор" или "обычный участник".
        /// </summary>
        public string Role { get; set; }

        // Навигационные свойства

        /// <summary>
        /// Пользователь, связанный с этой записью.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Группа, связанная с этой записью.
        /// </summary>
        public virtual Group Group { get; set; }
    }
}

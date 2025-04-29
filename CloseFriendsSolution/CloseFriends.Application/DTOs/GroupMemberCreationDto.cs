namespace CloseFriends.Application.DTOs
{
    /// <summary>
    /// DTO для добавления участника группы.
    /// </summary>
    public class GroupMemberCreationDto
    {
        /// <summary>
        /// Идентификатор группы, в которую нужно добавить участника.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Идентификатор пользователя, которого необходимо добавить в группу.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Роль участника в группе (например, "администратор" или "участник").
        /// Если не указано, используется дефолтное значение "Member".
        /// </summary>
        public string Role { get; set; }
    }
}

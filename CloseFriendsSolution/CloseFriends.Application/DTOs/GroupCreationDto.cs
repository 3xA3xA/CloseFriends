using CloseFriends.Domain.Entities;

namespace CloseFriends.Application.DTOs
{
    /// <summary>
    /// DTO для создания группы.
    /// Содержит данные для создания новой группы.
    /// </summary>
    public class GroupCreationDto
    {
        /// <summary>
        /// Название группы.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип группы, представленный как enum (Family, Couple, Friends).
        /// </summary>
        public GroupType Type { get; set; }

        /// <summary>
        /// URL изображения для группы.
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Описание группы.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Идентификатор владельца группы.
        /// </summary>
        public int OwnerId { get; set; }
    }
}

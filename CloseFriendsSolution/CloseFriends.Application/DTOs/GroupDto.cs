using System;

namespace CloseFriends.Application.DTOs
{
    /// <summary>
    /// DTO, содержащий данные группы, возвращаемые после успешного создания.
    /// </summary>
    public class GroupDto
    {
        /// <summary>
        /// Уникальный идентификатор группы.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название группы.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип группы в виде строки.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Идентификатор владельца группы.
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// URL изображения группы.
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Описание группы.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Сформированная уникальная ссылка для приглашения в группу.
        /// </summary>
        public string ShareLink { get; set; }

        /// <summary>
        /// Дата создания группы.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}

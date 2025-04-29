using CloseFriends.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CloseFriends.Application.DTOs
{
    /// <summary>
    /// DTO для создания группы.
    /// Содержит данные для создания новой группы.
    /// </summary>
    public class GroupCreationDto
    {
        /// <summary>
        /// Название группы, обязательное поле.
        /// </summary>
        [Required(ErrorMessage = "Название группы обязательно.")]
        public string Name { get; set; }

        /// <summary>
        /// Тип группы, обязательное поле.
        /// </summary>
        [Required(ErrorMessage = "Тип группы обязателен.")]
        public GroupType Type { get; set; }

        /// <summary>
        /// URL для фотографии группы. (Если указан – должен быть корректным URL.)
        /// </summary>
        [Url(ErrorMessage = "Неверный формат URL для фотографии.")]
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Описание группы.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Идентификатор владельца группы. Должен быть положительным числом.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "OwnerId должен быть положительным числом.")]
        public int OwnerId { get; set; }
    }
}

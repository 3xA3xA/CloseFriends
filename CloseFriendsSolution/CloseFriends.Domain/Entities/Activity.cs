using System;
using System.Collections.Generic;

namespace CloseFriends.Domain.Entities
{
    /// <summary>
    /// Перечисление статусов активности.
    /// </summary>
    public enum ActivityStatus
    {
        /// <summary>
        /// Активность завершена.
        /// </summary>
        Completed,
        /// <summary>
        /// Активность запланирована.
        /// </summary>
        Planning,
        /// <summary>
        /// Активность ещё не выполнена.
        /// </summary>
        NonCompleted
    }

    /// <summary>
    /// Представляет активность, которая запланирована для групп.
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// Уникальный идентификатор активности.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название активности.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Подробное описание активности.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Уровень сложности активности по шкале от 1 до 5.
        /// </summary>
        public int Difficulty { get; set; }

        /// <summary>
        /// Стоимость активности.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Продолжительность активности в минутах.
        /// </summary>
        public int DurationMinutes { get; set; }

        /// <summary>
        /// Статус активности: завершенная, запланированная или не выполненная.
        /// </summary>
        public ActivityStatus Status { get; set; }

        /// <summary>
        /// Дата создания активности.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления активности.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Навигационные свойства

        /// <summary>
        /// Коллекция записей о связях активности с группами.
        /// </summary>
        public virtual ICollection<ActivityItem> ActivityItems { get; set; } = new List<ActivityItem>();
    }
}

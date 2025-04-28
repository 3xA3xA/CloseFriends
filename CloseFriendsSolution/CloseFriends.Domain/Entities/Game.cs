using System;
using System.Collections.Generic;

namespace CloseFriends.Domain.Entities
{
    /// <summary>
    /// Представляет игру, то есть вопрос, заданный участникам группы.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Уникальный идентификатор игры.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Вопрос, который задаётся участникам (например, "Какой твой любимый цвет?").
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Категория игры. Можно задать фиксированный перечень значений через enum, если понадобится.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Дата создания игры.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления игры.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Навигационные свойства

        /// <summary>
        /// Коллекция ответов участников на этот вопрос.
        /// </summary>
        public virtual ICollection<GameAnswer> Answers { get; set; } = new List<GameAnswer>();
    }
}

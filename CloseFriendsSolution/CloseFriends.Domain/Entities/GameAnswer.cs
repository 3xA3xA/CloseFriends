using System;

namespace CloseFriends.Domain.Entities
{
    /// <summary>
    /// Представляет ответ пользователя на вопрос из игры.
    /// После того как пользователь отвечает, может быть инициировано уведомление другим участникам группы.
    /// </summary>
    public class GameAnswer
    {
        /// <summary>
        /// Уникальный идентификатор ответа.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Внешний ключ к вопросу игры, на который даётся ответ.
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// Внешний ключ к пользователю, который дал ответ.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Текст ответа пользователя.
        /// </summary>
        public string UserAnswer { get; set; }

        /// <summary>
        /// Дата и время, когда пользователь оставил ответ.
        /// </summary>
        public DateTime AnsweredAt { get; set; }

        // Навигационные свойства

        /// <summary>
        /// Игра, к которой относится ответ.
        /// </summary>
        public virtual Game Game { get; set; }

        /// <summary>
        /// Пользователь, который дал ответ.
        /// </summary>
        public virtual User User { get; set; }
    }
}

using System;

namespace CloseFriends.Domain.Entities
{
    /// <summary>
    /// Представляет уведомление, отправляемое участникам группы.
    /// Например, когда один из участников отвечает на вопрос в игре, другим может прийти уведомление с информацией о том, какой ответ был дан.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Уникальный идентификатор уведомления.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя, которому предназначено уведомление.
        /// </summary>
        public int RecipientUserId { get; set; }

        /// <summary>
        /// Идентификатор пользователя, который инициировал уведомление (например, тот, кто дал ответ).
        /// </summary>
        public int SenderUserId { get; set; }

        /// <summary>
        /// Идентификатор ответа (GameAnswer) или события, на основании которого сформировано уведомление.
        /// Этот параметр связывает уведомление с конкретным ответом.
        /// </summary>
        public int GameAnswerId { get; set; }

        /// <summary>
        /// Текст уведомления, который будет отображён пользователю.
        /// Например: "Ваш друг Иван ответил на вопрос 'Какой ваш любимый цвет?'"
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Флаг, показывающий, было ли уведомление прочитано.
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Дата и время создания уведомления.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        // Навигационные свойства

        /// <summary>
        /// Пользователь, получивший уведомление.
        /// </summary>
        public virtual User Recipient { get; set; }

        /// <summary>
        /// Пользователь, инициировавший уведомление.
        /// </summary>
        public virtual User Sender { get; set; }

        /// <summary>
        /// Ответ на игру, связанный с уведомлением.
        /// </summary>
        public virtual GameAnswer GameAnswer { get; set; }
    }
}

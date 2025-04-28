using System;
using System.Collections.Generic;

namespace CloseFriends.Domain.Entities
{
    /// <summary>
    /// Представляет пользователя приложения.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Уникальный идентификатор пользователя.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Электронная почта пользователя. Рекомендуется обеспечить уникальность этого поля.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Хэш пароля пользователя.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Дата регистрации пользователя.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления данных профиля.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Навигационные свойства

        /// <summary>
        /// Список записей участия в группах.
        /// </summary>
        public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();

        /// <summary>
        /// Список wish-листов, созданных пользователем.
        /// </summary>
        public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

        /// <summary>
        /// Список ответов пользователя на вопросы игр.
        /// </summary>
        public virtual ICollection<GameAnswer> GameAnswers { get; set; } = new List<GameAnswer>();

        /// <summary>
        /// Список уведомлений, полученных пользователем.
        /// </summary>
        public virtual ICollection<Notification> ReceivedNotifications { get; set; } = new List<Notification>();
    }
}

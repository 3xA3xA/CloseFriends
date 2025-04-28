using System;
using System.Collections.Generic;

namespace CloseFriends.Domain.Entities
{
    /// <summary>
    /// Представляет wish-лист, принадлежащий пользователю.
    /// </summary>
    public class Wishlist
    {
        /// <summary>
        /// Уникальный идентификатор wish-листа.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя, которому принадлежит wish-лист.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Заголовок или название wish-листа.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Дата создания wish-листа.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления wish-листа.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Навигационные свойства

        /// <summary>
        /// Пользователь, владеющий этим wish-листом.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Коллекция элементов wish-листа.
        /// </summary>
        public virtual ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
    }
}

using Microsoft.EntityFrameworkCore;
using CloseFriends.Domain.Entities;

namespace CloseFriends.Infrastructure.Data
{
    /// <summary>
    /// Контекст базы данных для приложения CloseFriends.
    /// Отвечает за сопоставление доменных сущностей с таблицами MSSQL.
    /// </summary>
    public class CloseFriendsContext : DbContext
    {
        public CloseFriendsContext(DbContextOptions<CloseFriendsContext> options)
            : base(options)
        { }

        // DbSet для каждой сущности. Эти свойства дадут EF Core представление о таблицах базы данных.

        /// <summary>
        /// Таблица пользователей.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Таблица групп (семья, пара, друзья).
        /// </summary>
        public DbSet<Group> Groups { get; set; }

        /// <summary>
        /// Таблица участников групп.
        /// </summary>
        public DbSet<GroupMember> GroupMembers { get; set; }

        /// <summary>
        /// Таблица wish-листов.
        /// </summary>
        public DbSet<Wishlist> Wishlists { get; set; }

        /// <summary>
        /// Таблица элементов wish-листа.
        /// </summary>
        public DbSet<WishlistItem> WishlistItems { get; set; }

        /// <summary>
        /// Таблица вопросов (игр).
        /// </summary>
        public DbSet<Game> Games { get; set; }

        /// <summary>
        /// Таблица ответов на игры.
        /// </summary>
        public DbSet<GameAnswer> GameAnswers { get; set; }

        /// <summary>
        /// Таблица активностей.
        /// </summary>
        public DbSet<Activity> Activities { get; set; }

        /// <summary>
        /// Таблица связей активностей с группами.
        /// </summary>
        public DbSet<ActivityItem> ActivityItems { get; set; }

        /// <summary>
        /// Таблица уведомлений.
        /// </summary>
        public DbSet<Notification> Notifications { get; set; }

        /// <summary>
        /// Метод для тонкой настройки схемы базы данных с помощью Fluent API.
        /// Здесь можно задать индексы, связи, ограничения и переопределить имена таблиц.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка уникального индекса для Email в таблице Users
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Настройка для навигационного свойства "Recipient" в Notification.
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Recipient)
                .WithMany(u => u.ReceivedNotifications)
                .HasForeignKey(n => n.RecipientUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка для навигационного свойства "Sender" в Notification.
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Sender)
                .WithMany() // нет свойства навигации в User для отправленных уведомлений
                .HasForeignKey(n => n.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка для сущности Activity
            modelBuilder.Entity<Activity>()
                .Property(a => a.Price)
                .HasPrecision(18, 2);

            // Настройка для сущности WishlistItem
            modelBuilder.Entity<WishlistItem>()
                .Property(w => w.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Notification>()
               .HasOne(n => n.GameAnswer)
               .WithMany()
               .HasForeignKey(n => n.GameAnswerId)
               .OnDelete(DeleteBehavior.Restrict);
        }

    }
}

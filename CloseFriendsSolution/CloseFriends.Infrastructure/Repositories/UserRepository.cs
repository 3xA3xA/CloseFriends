using CloseFriends.Application.Interfaces;
using CloseFriends.Domain.Entities;
using CloseFriends.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CloseFriends.Infrastructure.Repositories
{
    /// <summary>
    /// Реализация IUserRepository с использованием Entity Framework Core.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly CloseFriendsContext _context;

        /// <summary>
        /// Конструктор, получает объект DbContext через Dependency Injection.
        /// </summary>
        public UserRepository(CloseFriendsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Проверяет, существует ли пользователь с таким email.
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        /// <summary>
        /// Добавляет новую сущность пользователя в контекст.
        /// </summary>
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        /// <summary>
        /// Сохраняет изменения в базе данных.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

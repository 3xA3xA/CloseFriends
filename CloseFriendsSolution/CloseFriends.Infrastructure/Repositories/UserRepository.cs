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
        /// Проверяет, существует ли пользователь с указанным ID.
        /// </summary>
        public async Task<bool> ExistsByIdAsync(int userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        /// <summary>
        /// Получает пользователя по указанному email.
        /// </summary>
        /// <param name="email">Email пользователя, по которому производится поиск.</param>
        /// <returns>
        /// Возвращает сущность пользователя, если пользователь найден; 
        /// в противном случае возвращает null.
        /// </returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Добавляет новую сущность пользователя в контекст.
        /// </summary>
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
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

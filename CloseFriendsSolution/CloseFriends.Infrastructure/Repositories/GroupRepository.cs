using System.Threading.Tasks;
using CloseFriends.Application.Interfaces;
using CloseFriends.Domain.Entities;
using CloseFriends.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CloseFriends.Infrastructure.Repositories
{
    /// <summary>
    /// Реализация IGroupRepository с использованием Entity Framework Core.
    /// </summary>
    public class GroupRepository : IGroupRepository
    {
        private readonly CloseFriendsContext _context;

        /// <summary>
        /// Конструктор, внедряющий DbContext через Dependency Injection.
        /// </summary>
        public GroupRepository(CloseFriendsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавляет новую группу в контекст.
        /// </summary>
        public async Task AddAsync(Group group)
        {
            await _context.Groups.AddAsync(group);
        }

        /// <summary>
        /// Сохраняет изменения в базе данных.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Получает группу по её идентификатору.
        /// </summary>
        public async Task<Group> GetByIdAsync(int groupId)
        {
            return await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
        }

        /// <summary>
        /// Получает список всех групп.
        /// </summary>
        public async Task<IEnumerable<Group>> GetAllAsync()
        {
            return await _context.Groups.ToListAsync();
        }
    }
}

using System.Threading.Tasks;
using CloseFriends.Application.Interfaces;
using CloseFriends.Domain.Entities;
using CloseFriends.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CloseFriends.Infrastructure.Repositories
{
    /// <summary>
    /// Реализация IGroupMemberRepository с использованием Entity Framework Core.
    /// </summary>
    public class GroupMemberRepository : IGroupMemberRepository
    {
        private readonly CloseFriendsContext _context;

        public GroupMemberRepository(CloseFriendsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавляет нового участника группы в контекст.
        /// </summary>
        public async Task AddAsync(GroupMember member)
        {
            await _context.GroupMembers.AddAsync(member);
        }

        /// <summary>
        /// Проверяет, является ли пользователь участником указанной группы.
        /// </summary>
        public async Task<bool> IsMemberAsync(int groupId, int userId)
        {
            return await _context.GroupMembers.AnyAsync(m => m.GroupId == groupId && m.UserId == userId);
        }

        /// <summary>
        /// Получает всех участников групп.
        /// </summary>
        public async Task<IEnumerable<GroupMember>> GetAllAsync()
        {
            return await _context.GroupMembers.ToListAsync();
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

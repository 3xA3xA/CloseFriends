using System;
using System.Threading.Tasks;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;
using CloseFriends.Domain.Entities;

namespace CloseFriends.Application.Services
{
    /// <summary>
    /// Сервис для управления участниками групп.
    /// </summary>
    public class GroupMemberService : IGroupMemberService
    {
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;

        /// <summary>
        /// Конструктор, внедряющий зависимости.
        /// </summary>
        public GroupMemberService(
            IGroupMemberRepository groupMemberRepository,
            IUserRepository userRepository,
            IGroupRepository groupRepository)
        {
            _groupMemberRepository = groupMemberRepository;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }

        /// <summary>
        /// Добавляет нового участника в группу:
        /// 1) Проверяет существование группы.
        /// 2) Проверяет существование пользователя.
        /// 3) Проверяет, нет ли уже записи об участии.
        /// 4) Создаёт запись GroupMember (с учетом роли) и сохраняет изменения.
        /// </summary>
        public async Task<GroupMemberDto> AddMemberAsync(GroupMemberCreationDto dto)
        {
            // Проверка существования группы.
            var group = await _groupRepository.GetByIdAsync(dto.GroupId);
            if (group == null)
            {
                throw new ArgumentException("Группа не найдена.");
            }

            // Проверка существования пользователя.
            bool userExists = await _userRepository.ExistsByIdAsync(dto.UserId);
            if (!userExists)
            {
                throw new ArgumentException("Пользователь не найден.");
            }

            // Проверка: не является ли пользователь уже участником группы.
            bool alreadyMember = await _groupMemberRepository.IsMemberAsync(dto.GroupId, dto.UserId);
            if (alreadyMember)
            {
                throw new ArgumentException("Пользователь уже является участником группы.");
            }

            // Если роль не указана, устанавливаем дефолтное значение "Member".
            string role = string.IsNullOrWhiteSpace(dto.Role) ? "Member" : dto.Role;

            // Создание новой записи участника группы.
            var member = new GroupMember
            {
                GroupId = dto.GroupId,
                UserId = dto.UserId,
                Role = role,
                JoinedAt = DateTime.UtcNow
            };

            await _groupMemberRepository.AddAsync(member);
            await _groupMemberRepository.SaveChangesAsync();

            return new GroupMemberDto
            {
                Id = member.Id,
                GroupId = member.GroupId,
                UserId = member.UserId,
                Role = member.Role,
                JoinedAt = member.JoinedAt
            };
        }
    }
}

using System;
using System.Threading.Tasks;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;
using CloseFriends.Domain.Entities;

namespace CloseFriends.Application.Commands
{
    /// <summary>
    /// Обработчик команды создания участника группы.
    /// Выполняет валидацию: проверяет существование группы и пользователя, а также уникальность записи.
    /// </summary>
    public class CreateGroupMemberCommandHandler : ICreateGroupMemberCommandHandler
    {
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;

        public CreateGroupMemberCommandHandler(
            IGroupMemberRepository groupMemberRepository,
            IUserRepository userRepository,
            IGroupRepository groupRepository)
        {
            _groupMemberRepository = groupMemberRepository;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }

        public async Task<GroupMemberDto> HandleAsync(GroupMemberCreationDto dto)
        {
            // Проверка существования группы
            var group = await _groupRepository.GetByIdAsync(dto.GroupId);
            if (group == null)
                throw new ArgumentException("Группа не найдена.");

            // Проверка существования пользователя
            bool userExists = await _userRepository.ExistsByIdAsync(dto.UserId);
            if (!userExists)
                throw new ArgumentException("Пользователь не найден.");

            // Проверка, не является ли пользователь уже участником группы
            bool alreadyMember = await _groupMemberRepository.IsMemberAsync(dto.GroupId, dto.UserId);
            if (alreadyMember)
                throw new ArgumentException("Пользователь уже является участником группы.");

            // Если роль не указана, устанавливаем дефолтное значение "Member"
            string role = string.IsNullOrWhiteSpace(dto.Role) ? "Member" : dto.Role;

            // Создание новой сущности участника группы
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

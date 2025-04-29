using System;
using System.Threading.Tasks;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;
using CloseFriends.Domain.Entities;

namespace CloseFriends.Application.Commands
{
    /// <summary>
    /// Обработчик команды создания группы.
    /// </summary>
    public class CreateGroupCommandHandler : ICreateGroupCommandHandler
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;

        public CreateGroupCommandHandler(IGroupRepository groupRepository, IUserRepository userRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        public async Task<GroupDto> HandleAsync(GroupCreationDto dto)
        {
            // Валидация обязательного поля "Name"
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Название группы обязательно для заполнения.");

            // Проверка существования владельца группы
            bool ownerExists = await _userRepository.ExistsByIdAsync(dto.OwnerId);
            if (!ownerExists)
                throw new ArgumentException("Пользователь с таким OwnerId не существует.");

            // Генерация уникальной ссылки для приглашения (например, через GUID)
            string shareLink = Guid.NewGuid().ToString();

            // Создание новой сущности группы с использованием данных из DTO
            var group = new Group
            {
                Name = dto.Name,
                Type = dto.Type,  // В DTO тип уже задан как enum
                OwnerId = dto.OwnerId,
                PhotoUrl = dto.PhotoUrl,
                Description = dto.Description,
                ShareLink = shareLink,
                CreatedAt = DateTime.UtcNow
            };

            await _groupRepository.AddAsync(group);
            await _groupRepository.SaveChangesAsync();

            // Формируем ответный DTO с данными созданной группы
            return new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Type = group.Type.ToString(), // Преобразуем enum в строку для клиента
                OwnerId = group.OwnerId,
                PhotoUrl = group.PhotoUrl,
                Description = group.Description,
                ShareLink = group.ShareLink,
                CreatedAt = group.CreatedAt
            };
        }
    }
}

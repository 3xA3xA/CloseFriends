using System;
using System.Threading.Tasks;
using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;
using CloseFriends.Domain.Entities;

namespace CloseFriends.Application.Services
{
    /// <summary>
    /// Сервис для управления группами.
    /// </summary>
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Конструктор, внедряющий зависимость от IGroupRepository и IUserRepository.
        /// Принцип Dependency Inversion (D из SOLID).
        /// </summary>
        public GroupService(IGroupRepository groupRepository, IUserRepository userRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Создаёт новую группу:
        /// 1) Валидирует входные данные.
        /// 2) Генерирует уникальную ссылку для приглашения.
        /// 3) Сохраняет группу.
        /// 4) Возвращает DTO с данными созданной группы.
        /// </summary>
        public async Task<GroupDto> CreateGroupAsync(GroupCreationDto dto)
        {
            // Проверка обязательного поля "Name".
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Название группы обязательно для заполнения.");
            }

            // Проверка существования владельца группы.
            bool ownerExists = await _userRepository.ExistsByIdAsync(dto.OwnerId);
            if (!ownerExists)
            {
                throw new ArgumentException("Пользователь с таким OwnerId не существует.");
            }

            // Генерация уникальной ссылки для приглашения (например, с использованием GUID).
            // После можно переделать под адресную ссылку.
            string shareLink = Guid.NewGuid().ToString();

            // Создаём новую сущность группы.
            var group = new Group
            {
                Name = dto.Name,
                Type = dto.Type,  // Используем enum, полученный из DTO
                OwnerId = dto.OwnerId,
                PhotoUrl = dto.PhotoUrl,
                Description = dto.Description,
                ShareLink = shareLink,
                CreatedAt = DateTime.UtcNow
            };

            // Добавляем группу и сохраняем изменения.
            await _groupRepository.AddAsync(group);
            await _groupRepository.SaveChangesAsync();

            // Возвращаем данные созданной группы через DTO.
            return new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Type = group.Type.ToString(),  // Преобразуем enum в строку для удобства клиента
                OwnerId = group.OwnerId,
                PhotoUrl = group.PhotoUrl,
                Description = group.Description,
                ShareLink = group.ShareLink,
                CreatedAt = group.CreatedAt
            };
        }
    }
}

using CloseFriends.Application.DTOs;
using CloseFriends.Application.Interfaces;
using CloseFriends.Domain.Entities;
using BCrypt.Net; // для хэширования паролей

namespace CloseFriends.Application.Services
{
    /// <summary>
    /// Сервис для управления пользователями, реализует бизнес-логику регистрации и валидацию.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Конструктор, принимающий зависимость от IUserRepository.
        /// Это реализует принцип Dependency Inversion (D из SOLID).
        /// </summary>
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Регистрирует нового пользователя:
        /// 1) Проверяет уникальность email.
        /// 2) Хэширует пароль.
        /// 3) Создаёт сущность User и сохраняет её.
        /// 4) Возвращает DTO с данными зарегистрированного пользователя.
        /// </summary>
        public async Task<UserDto> RegisterUserAsync(UserRegistrationDto registrationDto)
        {
            // Проверка, существует ли уже пользователь с таким email.
            if (await _userRepository.EmailExistsAsync(registrationDto.Email))
            {
                throw new ArgumentException("Пользователь с таким email уже существует.");
            }


            // Хэширование пароля с использованием BCrypt.
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password);

            // Создаём новую сущность пользователя.
            var newUser = new User
            {
                Name = registrationDto.Name,
                Email = registrationDto.Email,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };

            // Добавляем пользователя в репозиторий.
            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            // Возвращаем данные нового пользователя через DTO.
            return new UserDto
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email
            };
        }

        /// <summary>
        /// Проверяет учетные данные пользователя по email и паролю.
        /// Метод производит поиск пользователя по email и сравнивает введённый пароль с хешем, 
        /// хранящимся в базе, используя BCrypt.Net.BCrypt.Verify.
        /// </summary>
        /// <param name="email">Email пользователя.</param>
        /// <param name="password">Введённый пароль.</param>
        /// <returns>
        /// Сущность пользователя, если введённый пароль соответствует хешу, иначе – null.
        /// </returns>
        public async Task<User> ValidateUserAsync(string email, string password)
        {
            // Получаем пользователя по email. Здесь используется метод GetUserByEmailAsync, который должен быть реализован в IUserRepository.
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                // Пользователь с указанным email не найден.
                return null;
            }

            // Сравниваем введённый пароль с хешированным паролем, сохранённым в базе.
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isPasswordValid)
            {
                // Пароль не совпадает.
                return null;
            }

            return user;
        }
    }
}

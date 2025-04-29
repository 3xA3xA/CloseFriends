using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CloseFriends.Infrastructure.Data;
using CloseFriends.Application.Interfaces;
using CloseFriends.Application.Services;
using CloseFriends.Infrastructure.Repositories;
using CloseFriends.Domain.Entities;

namespace CloseFriends.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Чтение строки подключения из конфигурации
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Регистрация DbContext для использования в приложении
            builder.Services.AddDbContext<CloseFriendsContext>(options =>
                options.UseSqlServer(connectionString));

            // Регистрация контроллеров
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                // Настройка сериализации для вывода enum в виде строк
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

            // Добавление контроллеров и других сервисов
            builder.Services.AddControllers();

            // Регистрация Swagger для визуализации и тестирования API
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Регистрация сервисов и репозиториев в соответствии с принципом Dependency Injection.
            // Это позволяет слабозависимые реализации и соблюдает принцип Inversion of Control.
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Регистрация сервисов и репозиториев в DI-контейнере
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                // Включаем Swagger UI, который будет доступен по адресу /swagger
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

//Справка 
//
//Domain: Содержит сущность User — чистую бизнес-модель без зависимостей от внешних библиотек.
//
//Application: Здесь определяются DTO для передачи данных между слоями, интерфейсы для сервисов и репозиториев, и реализуется бизнес-логика в UserService.
//Это обеспечивает изоляцию бизнес-правил и позволяет легко тестировать логику.
//
//Infrastructure: Здесь происходит реализация доступа к данным через репозитории (например, UserRepository), используя EF Core и CloseFriendsContext. Этот слой отделён от бизнес-логики через интерфейсы.
//
//API: Контроллеры являются тонким слоем, отвечающим за прием HTTP-запросов, управление валидацией, логированием и передачей управления в Application-сервис. 
//DI настроен так, чтобы зависимости разрешались автоматически согласно принципам SOLID.
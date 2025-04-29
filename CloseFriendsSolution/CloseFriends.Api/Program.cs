using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CloseFriends.Infrastructure.Data;
using CloseFriends.Application.Interfaces;
using CloseFriends.Application.Services;
using CloseFriends.Infrastructure.Repositories;
using CloseFriends.Domain.Entities;
using CloseFriends.Application.Queries;
using CloseFriends.Application.Commands;
using System.Reflection;
using Serilog;

namespace CloseFriends.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Чтение строки подключения из конфигурации
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ?? connectionString;


            // Регистрация DbContext для использования в приложении
            builder.Services.AddDbContext<CloseFriendsContext>(options =>
                options.UseSqlServer(connectionString));

            // Регистрация контроллеров
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                // Настройка сериализации для вывода enum в виде строк
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)  // Читает настройки из appsettings.json
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            builder.Host.UseSerilog();

            // Добавление контроллеров и других сервисов
            builder.Services.AddControllers();

            // Регистрация Swagger для визуализации и тестирования API
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
            });


            // Регистрация сервисов и репозиториев в соответствии с принципом Dependency Injection.
            // Это позволяет слабозависимые реализации и соблюдает принцип Inversion of Control.

            // Регистрация репозиториев в DI-контейнере
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IGroupMemberRepository, GroupMemberRepository>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();

            // Регистрация сервисовв DI-контейнере
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IGroupMemberService, GroupMemberService>();

            // Регистрация обработчиков запросов (CQRS)
            builder.Services.AddScoped<IGetGroupsQueryHandler, GetGroupsQueryHandler>();
            builder.Services.AddScoped<IGetUsersQueryHandler, GetUsersQueryHandler>();
            builder.Services.AddScoped<IGetGroupMembersQueryHandler, GetGroupMembersQueryHandler>();

            // Регистрация командных обработчиков (CQRS)
            builder.Services.AddScoped<ICreateGroupCommandHandler, CreateGroupCommandHandler>();
            builder.Services.AddScoped<ICreateGroupMemberCommandHandler, CreateGroupMemberCommandHandler>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                // Включаем Swagger UI, который будет доступен по адресу /swagger
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<CloseFriends.Api.Middleware.ExceptionHandlingMiddleware>(); //Для обработки глобальных исключений
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
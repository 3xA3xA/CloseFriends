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
using CloseFriends.Application.Settings;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


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

            // Настройка Serilog для логирования
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            builder.Host.UseSerilog();

            // Чтение настроек JWT из конфигурации
            var jwtSection = builder.Configuration.GetSection("Jwt");
            builder.Services.Configure<JwtSettings>(jwtSection);
            var jwtSettings = jwtSection.Get<JwtSettings>();

            // Регистрация аутентификации через JWT
            var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            


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
            builder.Services.AddScoped<ITokenService, TokenService>();

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

            app.UseMiddleware<CloseFriends.Api.Middleware.ExceptionHandlingMiddleware>(); //Для обработки глобальных исключений

            app.UseHttpsRedirection();
            app.UseAuthentication();
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
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

            // „тение строки подключени€ из конфигурации
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // –егистраци€ DbContext дл€ использовани€ в приложении
            builder.Services.AddDbContext<CloseFriendsContext>(options =>
                options.UseSqlServer(connectionString));

            // ƒобавление контроллеров и других сервисов
            builder.Services.AddControllers();

            // –егистрируем механизм дл€ исследовани€ API и генерации документации
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            // –егистраци€ сервисов и репозиториев в соответствии с принципом Dependency Injection.
            // Ёто позвол€ет слабозависимые реализации и соблюдает принцип Inversion of Control.
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.MapOpenApi();
            //}

            if (app.Environment.IsDevelopment())
            {
                // ¬ключаем Swagger UI, который будет доступен по адресу /swagger
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

//—правка 
//
//Domain: —одержит сущность User Ч чистую бизнес-модель без зависимостей от внешних библиотек.
//
//Application: «десь определ€ютс€ DTO дл€ передачи данных между сло€ми, интерфейсы дл€ сервисов и репозиториев, и реализуетс€ бизнес-логика в UserService.
//Ёто обеспечивает изол€цию бизнес-правил и позвол€ет легко тестировать логику.
//
//Infrastructure: «десь происходит реализаци€ доступа к данным через репозитории (например, UserRepository), использу€ EF Core и CloseFriendsContext. Ётот слой отделЄн от бизнес-логики через интерфейсы.
//
//API:  онтроллеры €вл€ютс€ тонким слоем, отвечающим за прием HTTP-запросов, управление валидацией, логированием и передачей управлени€ в Application-сервис. 
//DI настроен так, чтобы зависимости разрешались автоматически согласно принципам SOLID.
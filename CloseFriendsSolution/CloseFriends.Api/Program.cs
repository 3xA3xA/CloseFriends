using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CloseFriends.Infrastructure.Data;

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

            // Добавление контроллеров и других сервисов
            builder.Services.AddControllers();

            // Регистрируем механизм для исследования API и генерации документации
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            var app = builder.Build();

            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.MapOpenApi();
            //}

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

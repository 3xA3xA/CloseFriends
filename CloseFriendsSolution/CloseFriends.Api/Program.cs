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

            // ������ ������ ����������� �� ������������
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // ����������� DbContext ��� ������������� � ����������
            builder.Services.AddDbContext<CloseFriendsContext>(options =>
                options.UseSqlServer(connectionString));

            // ���������� ������������ � ������ ��������
            builder.Services.AddControllers();

            // ������������ �������� ��� ������������ API � ��������� ������������
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            // ����������� �������� � ������������ � ������������ � ��������� Dependency Injection.
            // ��� ��������� �������������� ���������� � ��������� ������� Inversion of Control.
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
                // �������� Swagger UI, ������� ����� �������� �� ������ /swagger
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

//������� 
//
//Domain: �������� �������� User � ������ ������-������ ��� ������������ �� ������� ���������.
//
//Application: ����� ������������ DTO ��� �������� ������ ����� ������, ���������� ��� �������� � ������������, � ����������� ������-������ � UserService.
//��� ������������ �������� ������-������ � ��������� ����� ����������� ������.
//
//Infrastructure: ����� ���������� ���������� ������� � ������ ����� ����������� (��������, UserRepository), ��������� EF Core � CloseFriendsContext. ���� ���� ������ �� ������-������ ����� ����������.
//
//API: ����������� �������� ������ �����, ���������� �� ����� HTTP-��������, ���������� ����������, ������������ � ��������� ���������� � Application-������. 
//DI �������� ���, ����� ����������� ����������� ������������� �������� ��������� SOLID.
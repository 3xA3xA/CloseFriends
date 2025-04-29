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

            // ����������� ������������
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                // ��������� ������������ ��� ������ enum � ���� �����
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

            // ���������� ������������ � ������ ��������
            builder.Services.AddControllers();

            // ����������� Swagger ��� ������������ � ������������ API
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ����������� �������� � ������������ � ������������ � ��������� Dependency Injection.
            // ��� ��������� �������������� ���������� � ��������� ������� Inversion of Control.

            // ����������� ������������ � DI-����������
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IGroupMemberRepository, GroupMemberRepository>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();

            // ����������� ��������� DI-����������
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IGroupService, GroupService>();
            builder.Services.AddScoped<IGroupMemberService, GroupMemberService>();

            var app = builder.Build();

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
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

            // ������ ������ ����������� �� ������������
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ?? connectionString;


            // ����������� DbContext ��� ������������� � ����������
            builder.Services.AddDbContext<CloseFriendsContext>(options =>
                options.UseSqlServer(connectionString));

            // ����������� ������������
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                // ��������� ������������ ��� ������ enum � ���� �����
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)  // ������ ��������� �� appsettings.json
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            builder.Host.UseSerilog();

            // ���������� ������������ � ������ ��������
            builder.Services.AddControllers();

            // ����������� Swagger ��� ������������ � ������������ API
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
            });


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

            // ����������� ������������ �������� (CQRS)
            builder.Services.AddScoped<IGetGroupsQueryHandler, GetGroupsQueryHandler>();
            builder.Services.AddScoped<IGetUsersQueryHandler, GetUsersQueryHandler>();
            builder.Services.AddScoped<IGetGroupMembersQueryHandler, GetGroupMembersQueryHandler>();

            // ����������� ��������� ������������ (CQRS)
            builder.Services.AddScoped<ICreateGroupCommandHandler, CreateGroupCommandHandler>();
            builder.Services.AddScoped<ICreateGroupMemberCommandHandler, CreateGroupMemberCommandHandler>();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                // �������� Swagger UI, ������� ����� �������� �� ������ /swagger
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<CloseFriends.Api.Middleware.ExceptionHandlingMiddleware>(); //��� ��������� ���������� ����������
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
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

            // ������ ������ ����������� �� ������������
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ?? connectionString;

            // ��������� Serilog ��� �����������
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            builder.Host.UseSerilog();

            // ������ �������� JWT �� ������������
            var jwtSection = builder.Configuration.GetSection("Jwt");
            builder.Services.Configure<JwtSettings>(jwtSection);
            var jwtSettings = jwtSection.Get<JwtSettings>();

            // ����������� �������������� ����� JWT
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
            builder.Services.AddScoped<ITokenService, TokenService>();

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

            app.UseMiddleware<CloseFriends.Api.Middleware.ExceptionHandlingMiddleware>(); //��� ��������� ���������� ����������

            app.UseHttpsRedirection();
            app.UseAuthentication();
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
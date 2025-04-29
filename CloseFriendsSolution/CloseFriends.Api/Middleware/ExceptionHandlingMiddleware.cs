using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CloseFriends.Api.Middleware
{
    /// <summary>
    /// Middleware для глобальной обработки исключений.
    /// Перехватывает необработанные исключения и возвращает корректный HTTP-код.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Необработанное исключение.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Определяем статус-код по типу исключения
            HttpStatusCode code = HttpStatusCode.InternalServerError;
            if (exception is ArgumentException)
            {
                code = HttpStatusCode.BadRequest;
            }
            else if (exception is KeyNotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            var errorResponse = JsonSerializer.Serialize(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            });
            return context.Response.WriteAsync(errorResponse);
        }
    }

    /// <summary>
    /// Модель данных для формирования ответа об ошибке.
    /// </summary>
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}

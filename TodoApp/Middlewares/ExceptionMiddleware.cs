using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TodoApp.BLL.Exceptions;

namespace TodoApp.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                BadRequestException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(
                    exception,
                    "Unhandled exception: {Message}",
                    exception.Message);
            }
            else
            {
                _logger.LogWarning(
                    "Handled exception {StatusCode}: {Message}",
                    statusCode,
                    exception.Message);
            }

            context.Response.StatusCode = statusCode;

            var response = new { message = exception.Message };
            string json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
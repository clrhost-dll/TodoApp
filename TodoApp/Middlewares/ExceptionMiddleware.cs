using System.Net;
using System.Text.Json;
using TodoApp.BLL.Exceptions;
    

namespace TodoApp.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(
                    context,
                    ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context,
            Exception exception)
        {
            context.Response.ContentType =
                "application/json";

            int statusCode = exception switch
            {
                NotFoundException =>
                    StatusCodes.Status404NotFound,

                UnauthorizedException =>
                    StatusCodes.Status401Unauthorized,

                BadRequestException =>
                    StatusCodes.Status400BadRequest,

                _ =>
                    StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                message = exception.Message
            };

            string json =
                JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}

using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using GithubStatistics.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GithubStatistics.WebAPI.Extensions
{
    public static class ExceptionExtension
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }

    public class CustomExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var status = HttpStatusCode.InternalServerError;
            object message = nameof(HttpStatusCode.InternalServerError);

            if (exception is CustomException customException)
            {
                status = customException.Status;
                message = customException.Message;
            }
            else
            {
                _logger.LogCritical(exception, "Unexpected Exception");
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var result = JsonSerializer.Serialize(new { status, message });
            return context.Response.WriteAsync(result);
        }
    }
}

using Application.Common.Exeptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace MyDreamShop.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception. Path: {Path}", context.Request.Path);
                await HandleExceptionAsync(context, ex, _environment);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            IHostEnvironment environment)
        {
            context.Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path
            };

            switch (exception)
            {
                case ValidationException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Validation Error";
                    problemDetails.Detail = ex.Message;
                    problemDetails.Extensions["errors"] = ex.Errors;
                    break;

                case UnauthorizedException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Status = StatusCodes.Status401Unauthorized;
                    problemDetails.Title = "Unauthorized";
                    problemDetails.Detail = ex.Message;
                    break;

                case ForbiddenException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    problemDetails.Status = StatusCodes.Status403Forbidden;
                    problemDetails.Title = "Forbidden";
                    problemDetails.Detail = ex.Message;
                    break;

                case NotFoundException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Not Found";
                    problemDetails.Detail = ex.Message;
                    break;

                case ConflictException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    problemDetails.Status = StatusCodes.Status409Conflict;
                    problemDetails.Title = "Conflict";
                    problemDetails.Detail = ex.Message;
                    break;

                case BusinessRuleException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Business Rule Violation";
                    problemDetails.Detail = ex.Message;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Internal Server Error";
                    problemDetails.Detail = environment.IsDevelopment()
                        ? exception.Message
                        : "Something went wrong.";
                    break;
            }

            var json = JsonSerializer.Serialize(problemDetails);

            await context.Response.WriteAsync(json);
        }
    }
}

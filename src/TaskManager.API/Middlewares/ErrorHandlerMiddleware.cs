using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskManager.Business.Common;

namespace TaskManager.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
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
                var response = context.Response;

                ApiResponse<string> responseModel;

                switch (ex)
                {
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        
                        var errors = e.Errors.Select(x => x.ErrorMessage).ToList();
                        responseModel = new ApiResponse<string>(e.Message);
                    break;

                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel = new ApiResponse<string>(e.Message);
                        break;

                    case InvalidOperationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel = new ApiResponse<string>(e.Message);
                    break;

                    default:
                        _logger.LogError(ex, ex.Message);
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel = new ApiResponse<string>("An error happend into the server");
                        break;
                }

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                await response.WriteAsync(JsonSerializer.Serialize(responseModel, options));
            }
        }
    }
}

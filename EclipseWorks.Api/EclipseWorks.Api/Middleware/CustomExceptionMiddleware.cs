using EclipseWorks.Domain.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.Net;

namespace EclipseWorks.Api.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;
        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "BadRequest");
                await HandleExceptionAsync(httpContext, "BadRequest", ex.Message, HttpStatusCode.BadRequest);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "NotFound");
                await HandleExceptionAsync(httpContext, "ArgumentNullException", ex.Message, HttpStatusCode.NotFound);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "BadRequest");
                await HandleExceptionAsync(httpContext, "BadRequest", ex.Message, HttpStatusCode.BadRequest);
            }            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception");
                await HandleExceptionAsync(httpContext, "Exception", "Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }


        private Task HandleExceptionAsync(HttpContext context, string title, string message, HttpStatusCode statusCode)
        {
            var result = BuildErrorResponse(title, message, statusCode);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = result.Status;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(result,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
        }

        private ExceptionResponse BuildErrorResponse(string title, string message, HttpStatusCode statusCode)
            => new ExceptionResponse(title, (int)statusCode, Activity.Current?.Id ?? "Unable to get TraceId", new Dictionary<string, List<string>>
                {
                    {
                        "ErrorDetails", new List<string>
                                    {
                                        message
                                    }
                    }
                });

    }
}

using EventScheduler.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace EventScheduler.Problems
{
    public class CustomErrorMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomErrorMiddleWare> _logger;
        public CustomErrorMiddleWare(RequestDelegate next,ILogger<CustomErrorMiddleWare> logger)
        {
            _next = next;
            _logger= logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception err)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            if (err is IEventSchedulerException exception)
            {
                if (exception.HttpStatusCode.HasValue)
                {
                    context.Response.StatusCode = (int)exception.HttpStatusCode.Value;
                }
                await context.Response.WriteAsync(exception.ToJson());
                _logger.LogError(exception.EventId, err, exception.Message);
            }
            else
            {
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Something went wrong"
                }));
            }
        }
    }
}


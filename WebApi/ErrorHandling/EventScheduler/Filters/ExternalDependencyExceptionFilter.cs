using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using EventScheduler.Services.Exceptions;

namespace EventScheduler.Filters
{
    
    public class ExternalDependencyExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ExternalDependencyException httpResponseException)
            {
                context.Result = new ObjectResult(httpResponseException.Value)
                {
                    StatusCode = (int)httpResponseException.StatusCode
                };

                context.ExceptionHandled = true; // this is critical, otherwise the controller will handle the exception and it will route to the exception page.
            }
        }
    }
}

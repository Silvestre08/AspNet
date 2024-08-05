using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace EventScheduler.Problems
{
    public class SampleProblemsFactory : ProblemDetailsFactory
    {

        private readonly ApiBehaviorOptions _options;
        public SampleProblemsFactory(
            IOptions<ApiBehaviorOptions> options)
        {
            _options = options?.Value;
        }

        public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        {
            

            var problem = new ProblemDetails
            {
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
                Status = statusCode??500,

            };
            if (_options != null)
            {
                if (_options.ClientErrorMapping.TryGetValue(statusCode.Value, out var clientErrorData))
                {
                  
                    problem.Title ??= clientErrorData.Title;
                    problem.Type ??= clientErrorData.Link;
                }
            }
            var traceId = httpContext?.TraceIdentifier;
            if (traceId != null)
            {
                problem.Extensions["traceId"] = traceId;
            }
            problem.Extensions.Add("MyCustom Value", "Value");
            problem.Extensions.Add("MyCustom Value2", new {PropertyOne="Test"});
            return problem;
        }
    

        public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        {
            return new ValidationProblemDetails(modelStateDictionary)
            {
                Title = title,
                Detail = detail,
                Status = statusCode,
                Instance = instance,
                Type = type,


            };
        }
    }
}

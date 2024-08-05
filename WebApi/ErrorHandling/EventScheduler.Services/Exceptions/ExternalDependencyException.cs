using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EventScheduler.Services.Exceptions
{
    public class ExternalDependencyException:Exception
    {
        public ExternalDependencyException(HttpStatusCode statusCode, object? value = null)
        {
            StatusCode=statusCode;
            Value = value;
        }

      
        public HttpStatusCode StatusCode { get; }

        public object? Value { get; }
    }
}

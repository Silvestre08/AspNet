using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EventScheduler.Common.Exceptions
{
    public class DefaultEventSchedulerException : Exception, IEventSchedulerException
    {
        public HttpStatusCode? HttpStatusCode { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }

        public EventId EventId => new EventId(100, "Default Error");

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new
            {
                StatusCode = HttpStatusCode,
                 ErrorCode = HttpStatusCode,
                 Message = Message
            });
        }
    }
}

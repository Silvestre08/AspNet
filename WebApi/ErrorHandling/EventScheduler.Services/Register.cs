using EventScheduler.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventScheduler.Services
{
    public static class Register
    {
        public static void RegisterEventSchedulerServices(this IServiceCollection serviceProvider)
        {
            //add all services in this library that need to be registered here.
            serviceProvider.AddScoped<IEventDataService, EventDataService>();
                
        }
    }
}

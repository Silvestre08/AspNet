using CityInfo.Domain.Services;
using CityInfo.Infrastructure.DbContexts;
using CityInfo.Infrastructure.Services;
using CityInfo.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CintyInfo.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddTransient<IMailService, LocalMailService>();
            services.AddDbContext<CityInfoContext>(options => options.UseSqlite(configuration["ConnectionStrings:CityInfoDBConnectionString"]));
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
        }
    }
}

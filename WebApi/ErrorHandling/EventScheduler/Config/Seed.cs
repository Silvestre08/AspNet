using EventScheduler.Data;
using EventScheduler.Data.Model;
using Microsoft.AspNetCore.Identity;

namespace EventScheduler.Config
{
    public static class UserInitializer
    {
        public static void SeedUsers(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!db.Database.CanConnect()) return;

                if (userManager.FindByNameAsync("john").Result == null)
                {
                    var user = new ApplicationUser();
                    user.UserName = "john";
                    user.Email = "test@eventscheduler.com";

                    var result = userManager.CreateAsync
                    (user, "John@123#").Result;


                }


                if (userManager.FindByNameAsync("jane").Result == null)
                {
                    var user = new ApplicationUser();
                    user.UserName = "jane";
                    user.Email = "jane@eventscheduler.com";

                    var result = userManager.CreateAsync
                    (user, "Jane@123#").Result;
                }
            }

           
        }
    }
}

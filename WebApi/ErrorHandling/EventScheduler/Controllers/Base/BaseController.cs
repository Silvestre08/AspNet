using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventScheduler.Controllers.Base
{
    public class BaseController : ControllerBase
    {

        public string CurrentUserId
        {
            get
            {
                if (User.Identity?.IsAuthenticated==false)
                {
                    throw new Exception("Unauthorized cannot get user id");
                }
                var userId = User.Claims.FirstOrDefault(z => z.Type == ClaimTypes.Sid)?.Value;
                if (string.IsNullOrEmpty(userId)) throw new Exception("User id not found");
                return userId;
            }
        }
    }
}

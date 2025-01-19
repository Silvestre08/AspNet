using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.Client.Controllers
{
    public class AuthenticationController : Controller
    {
        [Authorize]
        public async Task Logout()
        {
            // to logout the application must clear the cookie.
            // the scheme name must match the name we used to configure the authentication middleware.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // redirects to the IDP linked to the scheme 
            // OpenIdConnectDefaults.AuthenticationScheme so it clears the session
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        // returns a view access denied
        public IActionResult AccessDenied() 
        {
            return View();
        }
  
    }
}

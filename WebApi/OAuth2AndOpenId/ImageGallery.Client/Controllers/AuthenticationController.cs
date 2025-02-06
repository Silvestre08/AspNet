using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ImageGallery.Client.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthenticationController(IHttpClientFactory httpClientFactory)
        {    
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        public async Task Logout()
        {
            var client = _httpClientFactory.CreateClient("IDPClient");
            var discoveryDocument = await client.GetDiscoveryDocumentAsync();
            if (discoveryDocument.IsError)
            {
                throw new Exception(discoveryDocument.Error);
            }

           
            await client.RevokeTokenAsync(new TokenRevocationRequest
            {
                Address = discoveryDocument.RevocationEndpoint,
                ClientId = "imagegalleryclient",
                ClientSecret = "secret",
                Token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken)

            });


            await client.RevokeTokenAsync(new TokenRevocationRequest
            {
                Address = discoveryDocument.RevocationEndpoint,
                ClientId = "imagegalleryclient",
                ClientSecret = "secret",
                Token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken)

            });

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

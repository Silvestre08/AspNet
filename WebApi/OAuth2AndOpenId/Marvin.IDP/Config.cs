using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Marvin.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", "Your role(s)", new [] { "role" }),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("imagegalleryapi.fullaccess")
            };

    public static IEnumerable<ApiResource> ApiResources =>
    new ApiResource[]
    {
        new ApiResource("imagegalleryapi", "Image Gallerey API", new []{ "role" })
        {
            Scopes = { "imagegalleryapi.fullaccess" }
        }
    };

    public static IEnumerable<Client> Clients =>
        new Client[] 
            { new Client { ClientName = "Image Gallery" , 
                ClientId = "imagegalleryclient", // client app identifier
                AllowedGrantTypes = GrantTypes.Code, // authorization code flow
                RedirectUris = { "https://localhost:7184/signin-oidc" }, // client redirect uri
                PostLogoutRedirectUris = { "https://localhost:7184/signout-callback-oidc" },
                AllowedScopes = 
                { 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "roles",
                    "imagegalleryapi.fullaccess"
                },
                ClientSecrets = { new Secret("secret".Sha256()) },
                RequireConsent = true,
                
            }

            };
}
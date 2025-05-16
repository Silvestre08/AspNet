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
            new IdentityResource("country", "The country you are living in", new [] { "country" }),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("imagegalleryapi.fullaccess"),
                new ApiScope("imagegalleryapi.read"),
                new ApiScope("imagegalleryapi.write")
            };

    public static IEnumerable<ApiResource> ApiResources =>
    new ApiResource[]
    {
        new ApiResource("imagegalleryapi", "Image Gallerey API", new []{ "role", "country"})
        {
            Scopes = { "imagegalleryapi.fullaccess", "imagegalleryapi.read", "imagegalleryapi.write" },
            ApiSecrets =  { new Secret("apisecret".Sha256())},
        }
    };

    public static IEnumerable<Client> Clients =>
        new Client[] 
            { new Client { ClientName = "Image Gallery" , 
                ClientId = "imagegalleryclient", // client app identifier
                AllowedGrantTypes = GrantTypes.Code, // authorization code flow
                RedirectUris = { "https://localhost:7184/signin-oidc" }, // client redirect uri
                PostLogoutRedirectUris = { "https://localhost:7184/signout-callback-oidc" },
                UpdateAccessTokenClaimsOnRefresh = true, // Refresh the claims
                AllowOfflineAccess = true,
                AccessTokenType = AccessTokenType.Reference,
                //IdentityTokenLifetime = 300
                //AuthorizationCodeLifetime = 300
                AccessTokenLifetime = 120,
                AllowedScopes = 
                { 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "roles",
                    //"imagegalleryapi.fullaccess",
                    "imagegalleryapi.read",
                    "imagegalleryapi.write",
                    "country"
                },
                ClientSecrets = { new Secret("secret".Sha256()) },
                RequireConsent = true,    
            }
            };
}
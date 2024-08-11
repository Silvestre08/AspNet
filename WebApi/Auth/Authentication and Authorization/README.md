# Authentication and Authorizarion in ASP.NET

APIs manage data and or business logic for applications; therefore they are often the heart of an organization!
As such they need to be protected.
This folder of authentication and authorization has several projects, split by folders, each project demonstrate a differente type of authentication.

# Protecting APIs with keys

The most basic way of protecting APIs is by using a key.
A key is like a password that must be supplied by the consumer of an API to gain access.
The consumer is typically another application. An http header is often used to do that. The browser in our api has nothing to do with a key (stored in the API.). It is just used for machine to machine.

Problems with API keys

1. Keys can easily be stolen (usually laying around excel sheets, etc.)
1. Tend to have no expiraltion (hard to revoke when the attacker gets it).
1. All parties that have the key would have to rotate at the same time.
1. No middleware that supports API keys.

There are often better, more modern ad secure options. It is just mentioned here for infomation and completeness purposes.
Api keys can only offer protection on a basic level.
The Api key comes in the http header.
We can protect the api with middleware or API key attribute.
The key coming in the header must come from our mvc app. The key should be stored as a secret. It is fine for demonstration to just put it in the appSettings. In production, we should use secrete storage.

An Api key middleware would look like this:

```
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _ApiKeyName = "XApiKey";
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IConfiguration config)
        {
            var apiKeyPresentInHeader = context.Request.Headers
                .TryGetValue(_ApiKeyName, out var extractedApiKey);
            var apiKey = config[_ApiKeyName];

            if ((apiKeyPresentInHeader && apiKey == extractedApiKey)
                || context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid Api Key");
        }
    }
```

The key comes in the http request headers and it is share with the client app. The client app, in our case the MVC app, we configure the http client to send the key in the headers:

```
builder.Services.AddSingleton(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri("https://localhost:5002") };
    client.DefaultRequestHeaders.Add("XApiKey", "secret");
    return client;
});
```

The middleware needs to be configured in _Program.cs_ and we need to define before the authorization middleware (the order in program.cs matters):

```
 public static void UseApiKeyAuthentication(this IApplicationBuilder webApplication)
 {
     webApplication.UseMiddleware<ApiKeyMiddleware>();
 }
```

With similar code we can create an API key attribute and we can decorate pretty much anything with it: controller level, lambda functions (in case of minimal apis) or individual controller actions.

```
    [AttributeUsage(AttributeTargets.Class |
        AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiKeyAttribute : Attribute, IAuthorizationFilter
    {
        private const string _ApiKeyName = "XApiKey";
        private readonly IConfiguration _Config;

        public ApiKeyAttribute(IConfiguration config)
        {
            _Config = config;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var apiKeyPresentInHeader = httpContext.Request.Headers
                .TryGetValue(_ApiKeyName, out var extractedApiKey);
            var apiKey = _Config[_ApiKeyName];

            if ((apiKeyPresentInHeader && apiKey == extractedApiKey)
                || httpContext.Request.Path.StartsWithSegments("/swagger"))
            {
                return;
            }

            context.Result = new UnauthorizedResult();
        }
    }
```

The attribute needs to be referenced like this because it uses dependency injection :

```
    [ApiController]
    [Route("conference")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [TypeFilter(typeof(ApiKeyAttribute))]
    public class ConferenceController : Controller
    {}
```

The API can work with multiple keys, usually one different key for each external party.

# Cookie authentication

If Apis need to know who the user is, keys won't work.
One ways to send data in a safe way to the APIs is by using cookies. To get an identity cookie users first have to present proof of who they are.
This is the authentication or AuthN process.
After authentication we usually have authorization or AuthZ where we limit access to the application based on who they are (roles, etc.)

Once the user authenticates in the login page, the app sends a cookie to the browser. The browser stores the cookie and sends it in subsequent requests.
The cookie can only be sent to the API if both the front-end and back-end are in similar domains.
![](doc/cookie.png)
This would tightly couple the front-end and back end. Other front ends could not access the same API.
This is a problem in MVC because MVC is a server rendered application.
The alternative is to use a monolith where the MVC application, not the API, handles the data itself. Separating in a api does not bring value.

The best alternative is to use a SPA, if we are using cookies, is to use a single page application, rendered in the browser. So then the back-end API becomes responsible for the authentication itself, providing a login page itself, and handle the authentication, because is directly accessible by the browser.
The backend can host a login page. The backend is more than an API now and it becomes the server side application:

![](doc/cookieSpa.png)
We included an example in Blazor under the folder cookieSPA. Some notes to pay attention while inspecting the solution: the sever application is referencing the client application. The server application servers the login page. In blazor the sections that require authorization are under the Authorization view.

The server application is a webAPI with an account controller that returns the view to the browser.
The way to ensure the API handles authentication properly is to decorate the controllers with the Authorize attribute. The authorize attribute can receive as a parameter the authentication scheme (the way we autheticate, key, cookie, etc), if there is more than one.

There is a way to declare the authorize attribute gloaly, via an Authorize filter.
To disable we can use the [AllowAnonimous] as well.
While on swagger, we can navigate to the login page, enter credentialsand then go back to swagger because the cookie will be set in the browser already.
The cookie contains the claims and the rest of the information to create the ClaimsPrincipal object, that is accessible from the controller base API.

# Token authentication and identity
What about other apps like desktop apps? They do not work with cookies.
Antoher authenication scheme, Bearer toker scheme, can be used.
To keep in mind that we can have more than just one authentication scheme.
The claims information is now sent via the token. When the user logs in, the token is issued.
Tokens do not need a browser and they are not restricted to the same domain.
We checked in the web api fundamentals course, we've seen already the bearer token authentication.
In asp.net core 8, it was introduced ASP.COre identity, that comes with EF core out of the box solution to manage users, roles, etc. 
Whem we have architectures like this tokens become a nightmare to manage:

![](doc/multipleapis.PNG)
Each api needs to check the token, login, etc
This is where OAuth2 and OpenIdConnect come into play.

# OpenId connect and OAuth2
OAuth2 is an industry standard that uses tokens. It is ideal for distributed applications.
OAuth2 is a standar that describes the format of an access token and how to obtain it.
They are not used like the Bearer tokens of the previous section.
The main difference between the ASP.NET core tokens and OAuth2 tokens is that with OAuth2 there is a third party that provides the token, the *Identity Provider * aka STS (secure token servers).

The identity provider is a service application, a special kind of API in the application whose job is to hand out token to applications that need them.
There are several token types like access token for APIs and identity tokens for front-ends.

One of the ways to obtain access token is with the *Client Credentials Flow*. This flow is used with Machine to Machine authentication.
Here is the sequence diagram of the client credentials flow:

![](doc/clientCredentialsFlow.PNG)

To obtain an access token, the client needs to send the ClientId and a Secret (application credentials) to the identity provider.
These credentials have nothing to do with users.
Compared to protect the API with api keys, there are several differences:
1. The access token has a limited lifetime (controlled by the Identity providers)
1. Multiple APIS can be protected at the same time.
1. It is protected by encryption.

Let's dig into the solution present in the folder *ClientCreadentialsFlow*

'''
// here we configure the JWT Bearer token as the default authentication scheme
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.Authority = "https://localhost:5001"; // where the identiy provider is located.
        o.TokenValidationParameters.ValidateAudience = false; // in case the verification that the token came from the authority is not enoug
        o.TokenValidationParameters.ValidTypes = new[] { "at+jwt" }; // we only accept JWTs
    });
'''

The previous lines of code tell the API to use JWT tokens. Bearer means that every request bearing the token will be granted access.
The API will only accept tokens that were issued by the defined authority. It is able to do it by verifying cryptographic characteristics present on the token.
In this scenarion we disabled the audience because the check we do of the tokens authority is enough.
The token can contain a claim that contains the name of the API the token is meant for. If the audience check is done, the claim is checked agains the configured name of the API.
This is a microsfot implementation. We disabled it here because this is not part of the Auth2 standard.
Because this is not the OAuth2 default, the audience claim does not come by default.
We can do that with a policy instead.
We need to add security definitions for the Swagger UI:

'''
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {   
                TokenUrl = new Uri("https://localhost:5001/connect/token"), // where tio obtain the token
                Scopes = new Dictionary<string, string>
                {
                    { "globoapi", "Access to Globomantics API" },
                }
            },  
            
        }
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    { 
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "oauth2", //The name of the previously defined security scheme.
                    Type = ReferenceType.SecurityScheme
                }
            },new List<string>()
        }
    });
});
'''

We created our own identiy provider with a framework called identity server. It is free for dev and testing, but paid for productions. Do not implement your owns security solution even if you have to pay for tools like identity server.
There are alternatives to the identity server like the OpenIdDict.
Inspecting the code of identity server, we see identity server does not come uo with its own users store. It was created based on a template provided by Duende.
It has a helper method to add test users.
What are identity resources and scopes and clients?

```
   isBuilder.AddInMemoryIdentityResources(Config.IdentityResources);
   isBuilder.AddInMemoryApiScopes(Config.ApiScopes);
   isBuilder.AddInMemoryClients(Config.Clients);
``` 

A scope is a string that represents either a collection of user claims or access to an API.
The collection of claims is called the *Identity scope*
The APi scope represents the physical API we want to protect, which in this case it is just ourAPI: lower case is the convention.
Client is an applications that wants tokens. See the example of a client:
```
new Client[]
{
    // m2m client credentials flow client
    new Client
    {
        ClientId = "m2m.client",
        ClientName = "Client Credentials Client",

        AllowedGrantTypes = GrantTypes.ClientCredentials, // grants are tied to flows. We are granting the client credentials flow to this client.
        ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) }, // not a good idea to have this in source control

        AllowedScopes = { "globoapi" },   // the client is only allowed thi API.
        
    },
}
    ```
We can configure the access lifetime of a token here as well.
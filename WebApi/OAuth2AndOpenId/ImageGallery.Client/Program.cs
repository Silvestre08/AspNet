using ImageGallery.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(configure => 
        configure.JsonSerializerOptions.PropertyNamingPolicy = null);

// create an HttpClient used for accessing the API
builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ImageGalleryAPIRoot"]);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
}).AddUserAccessTokenHandler();
builder.Services.AddHttpClient("IDPClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:44300");
});

JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddOpenIdConnectAccessTokenManagement();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => 
{
    options.AccessDeniedPath = "/Authentication/AccessDenied";
})
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options => 
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.Authority = "https://localhost:44300/";
    options.ClientId = "imagegalleryclient";
    options.ClientSecret = "secret";
    options.ResponseType = "code";
    // these scopes are automatically requested by the middleware
    //options.Scope.Add("openid");
    //options.Scope.Add("profile");
    //options.CallbackPath = new PathString("signin-oidc");
    options.SaveTokens = true; // configure middleware to save tokens it receives
    options.GetClaimsFromUserInfoEndpoint = true;
    options.ClaimActions.Remove("aud");
    options.ClaimActions.DeleteClaim("sid");
    options.Scope.Add("roles");
    options.Scope.Add("country");
    options.Scope.Add("offline_access");
    //options.Scope.Add("imagegalleryapi.fullaccess");
    options.Scope.Add("imagegalleryapi.read");
    options.Scope.Add("imagegalleryapi.write");
    options.ClaimActions.MapJsonKey("role", "role");
    options.ClaimActions.MapUniqueJsonKey("country", "country");
    options.TokenValidationParameters = new()
    {
        NameClaimType =  "given_name",
        RoleClaimType = "role"

    };
});
builder.Services.AddAuthorization(options => { options.AddPolicy("UserCanAddImage", AuthorizationPolicies.CanAddImage()); });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gallery}/{action=Index}/{id?}");

app.Run();

using Asp.Versioning.ApiExplorer;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CityInfo.Api.DbContexts;
using CityInfo.Api.Services;
using CityInfo.API.Services;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    //.WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (environment == Environments.Development)
{
    builder.Host.UseSerilog(
        (context, loggerConfiguration) => loggerConfiguration
            .MinimumLevel.Debug()
            .WriteTo.Console());
}
else
{
    var secretClient = new SecretClient(
            new Uri("https://kelvaultexperiment.vault.azure.net/"),
            new DefaultAzureCredential());
    builder.Configuration.AddAzureKeyVault(secretClient,
        new KeyVaultSecretManager());


    builder.Host.UseSerilog(
        (context, loggerConfiguration) => loggerConfiguration
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.ApplicationInsights(new TelemetryConfiguration
            {
                InstrumentationKey = builder.Configuration["ApplicationInsightsInstrumentationKey"]
            }, TelemetryConverter.Traces));
}


//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;

}).AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();

builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddTransient<IMailService, LocalMailService>();
builder.Services.AddDbContext<CityInfoContext>(options => options.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
               //Convert.FromBase64String("c187nb9P1ad94deWVO3QAAEF1DhYjuKpCsmDUjGbZLQ=")
               Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"])
               )
        };
    }
    );
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeFromAntwerp", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("city", "Antwerp");
    });
});

// configure swagger documentation
builder.Services.AddApiVersioning(versionSettings => 
{
    versionSettings.ReportApiVersions = true;
    versionSettings.AssumeDefaultVersionWhenUnspecified = true;
    versionSettings.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
}).AddMvc()
.AddApiExplorer(setup => { setup.SubstituteApiVersionInUrl = true; }); // while doing routed versioning, this substitutes the version parameter directly in the documentation.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// we need to call this so we can access the api version description provider.
var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
builder.Services.AddSwaggerGen(setupAction =>
{

    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        setupAction.SwaggerDoc($"{description.GroupName}", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "City Info API",
            Version = description.ApiVersion.ToString(),
            Description = "Through this API you can access cities and their points of interest"
        });
    }

    // tell swashbuckler where to find the XML docs of the assembly api
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
    setupAction.IncludeXmlComments(xmlCommentsFilePath);

    setupAction.AddSecurityDefinition("CityInfoApiBearerAuth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme 
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
    });
    setupAction.AddSecurityRequirement(new()
    {
        {
            new ()
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "CityInfoApiBearerAuth" }
            },
            new List<string>()
        }
    });
});



//builder.Services.AddProblemDetails(problemDetails =>
//problemDetails.CustomizeProblemDetails = ctx => {
//    ctx.ProblemDetails.Extensions.Add("additionalInfor", "AdditionalInfoExample");
//    ctx.ProblemDetails.Extensions.Add("Server", Environment.MachineName);
//});

builder.Services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | 
Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(setupAction => 
    {
        // creates a new swagger endpoint per API versioning. 
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions) 
        {
            setupAction.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
//}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// new way of doing instead of using endpoitns
app.MapControllers();

app.Run();

using CityInfo.API;
using CityInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;

}).AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddTransient<IMailService, LocalMailService>();
builder.Services.AddSingleton<CitiesDataStore>();

//builder.Services.AddProblemDetails(problemDetails =>
//problemDetails.CustomizeProblemDetails = ctx => {
//    ctx.ProblemDetails.Extensions.Add("additionalInfor", "AdditionalInfoExample");
//    ctx.ProblemDetails.Extensions.Add("Server", Environment.MachineName);
//});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
// new way of doing instead of using endpoitns
app.MapControllers();

app.Run();

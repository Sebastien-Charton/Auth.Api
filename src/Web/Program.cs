using System.Globalization;
using Auth.Api.Application;
using Auth.Api.Infrastructure;
using Auth.Api.Infrastructure.Data;
using Auth.Api.Web;
using Auth.Api.Web.Infrastructure.Logging;
using Microsoft.AspNetCore.Localization;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{environment}.secrets.json", false, true)
    .AddJsonFile($"appsettings.{environment}.json", false, true)
    .Build();

builder.Host.UseSerilog(Serilogger.Configure);

builder.Services.AddKeyVaultIfConfigured(builder.Configuration);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

var defaultCulture = builder.Configuration
    .GetValue<string>("CultureInfo:DefaultCulture")!;

var supportedCultures = builder.Configuration
    .GetSection("CultureInfo:SupportedCultures")
    .Get<string[]>()!
    .Select(x => new CultureInfo(x))
    .ToList();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(defaultCulture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRequestLocalization();

app.UseSwaggerUi3(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });

app.Map("/", () => Results.Redirect("/api"));

app.MapEndpoints();

app.Run();

namespace Auth.Api.Web
{
    public class Program
    {
    }
}

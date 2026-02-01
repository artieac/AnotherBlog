using AlwaysMoveForward.AnotherBlog.Web;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;
using AlwaysMoveForward.AnotherBlog.Web.Configuration;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.Common.Utilities;
using Microsoft.Extensions.Options;
using AlwaysMoveForward.Common.Configuration;
using AlwaysMoveForward.AnotherBlog.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Add file logging (using a simple custom provider or third-party package)
// For production, consider adding: builder.Logging.AddFile("Logs/AnotherBlog-{Date}.log");

// Add services to the container
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CookieAuthenticationFilter>();
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add data protection
builder.Services.AddDataProtection();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AlwaysMoveForward.AnotherBlog.Web.Code.Utilities.PageManager>();

// Register application configuration
builder.Services.Configure<WebSiteSettings>(builder.Configuration.GetSection("AnotherBlog"));
builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection("AlwaysMoveForward:Database"));
builder.Services.Configure<OAuthSettings>(builder.Configuration.GetSection("AlwaysMoveForward:OAuth"));
builder.Services.Configure<Auth0Settings>(builder.Configuration.GetSection("AlwaysMoveForward:Auth0"));

// Add HttpClient for Auth0 API calls
builder.Services.AddHttpClient();

DatabaseConfiguration databaseConfiguration = new DatabaseConfiguration();
builder.Configuration.GetSection("AlwaysMoveForward:Database").Bind(databaseConfiguration);

//DatabaseConfiguration databaseConfiguration = builder.Configuration.GetValue<DatabaseConfiguration>("AlwaysMoveForward:Database");

// Register ServiceManagerBuilder
builder.Services.AddScoped<ServiceManagerBuilder>(sp =>
{
    var dbSettings = sp.GetRequiredService<IOptions<DatabaseConfiguration>>();
    return new ServiceManagerBuilder(dbSettings);
});

var app = builder.Build();

// Configure the LogManager with the application's ILoggerFactory
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
LogManager.Configure(loggerFactory);

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Index");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Serve static files from Content folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        System.IO.Path.Combine(builder.Environment.ContentRootPath, "Content")),
    RequestPath = "/Content"
});

// Serve static files from Scripts folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        System.IO.Path.Combine(builder.Environment.ContentRootPath, "Scripts")),
    RequestPath = "/Scripts"
});

// Default wwwroot (if it exists)
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Map routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Site}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "AdminBlogSubFolder",
    pattern: "Admin/{controller}/{action}/{blogSubFolder}/{id?}");

app.MapControllerRoute(
    name: "PostSpecific",
    pattern: "Admin/{controller}/{action}/{blogSubFolder}/{blogPostId}/{filter?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Initialize site info
WebApplicationState.Initialize(app.Services);

app.Run();

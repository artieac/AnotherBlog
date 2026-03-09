using AlwaysMoveForward.AnotherBlog.Web;
using AlwaysMoveForward.AnotherBlog.Web.Code.Filters;
using AlwaysMoveForward.AnotherBlog.Web.Configuration;
using AlwaysMoveForward.AnotherBlog.BusinessLayer.Service;
using AlwaysMoveForward.Common.Utilities;
using Microsoft.Extensions.Options;
using AlwaysMoveForward.Common.Configuration;
using AlwaysMoveForward.Common.Encryption;
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
builder.Services.Configure<OAuthSettings>(builder.Configuration.GetSection("AlwaysMoveForward:OAuth"));
builder.Services.Configure<Auth0Settings>(builder.Configuration.GetSection("AlwaysMoveForward:Auth0"));

// Register encryption configuration
builder.Services.Configure<AESConfiguration>(builder.Configuration.GetSection(AESConfiguration.DEFAULT_SECTION));
builder.Services.Configure<KeyFileConfiguration>(builder.Configuration.GetSection(KeyFileConfiguration.DEFAULT_SECTION));
builder.Services.Configure<KeyStoreConfiguration>(builder.Configuration.GetSection(KeyStoreConfiguration.DEFAULT_SECTION));
builder.Services.Configure<RSAXmlKeyFileConfiguration>(builder.Configuration.GetSection(RSAXmlKeyFileConfiguration.DEFAULT_SECTION));

// Register DatabaseConfiguration with its encryption dependencies
builder.Services.AddSingleton<IOptions<DatabaseConfiguration>>(sp =>
{
    var aesConfiguration = sp.GetRequiredService<IOptions<AESConfiguration>>();
    var keyFileConfiguration = sp.GetRequiredService<IOptions<KeyFileConfiguration>>();
    var keyStoreConfiguration = sp.GetRequiredService<IOptions<KeyStoreConfiguration>>();
    var rsaXmlKeyFileConfiguration = sp.GetRequiredService<IOptions<RSAXmlKeyFileConfiguration>>();

    var dbConfig = new DatabaseConfiguration(aesConfiguration, keyFileConfiguration, keyStoreConfiguration, rsaXmlKeyFileConfiguration);

    // Bind the configuration values from appsettings
    builder.Configuration.GetSection(DatabaseConfiguration.DEFAULT_SECTION).Bind(dbConfig);

    return Options.Create(dbConfig);
});

// Add HttpClient for Auth0 API calls
builder.Services.AddHttpClient();

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
var contentPath = System.IO.Path.Combine(builder.Environment.ContentRootPath, "Content");
if (System.IO.Directory.Exists(contentPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(contentPath),
        RequestPath = "/Content"
    });
}

// Serve static files from Scripts folder
var scriptsPath = System.IO.Path.Combine(builder.Environment.ContentRootPath, "Scripts");
if (System.IO.Directory.Exists(scriptsPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(scriptsPath),
        RequestPath = "/Scripts"
    });
}

// Default wwwroot (if it exists)
var wwwrootPath = System.IO.Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
if (System.IO.Directory.Exists(wwwrootPath))
{
    app.UseStaticFiles();
}

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

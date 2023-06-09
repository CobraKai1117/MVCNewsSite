using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCNewsSite.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using MVCNewsSite.Services;
using Microsoft.AspNetCore.Routing.Patterns;
using MaxMind.GeoIP2;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var config = new AppConfiguration();
config.ApiKey = configuration["ApiKey"];
config.weatherApiKey = configuration["WeatherApiKey"];
builder.Services.AddSingleton<AppConfiguration>(config);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
    googleOptions.CorrelationCookie.Domain = "localhost";
    googleOptions.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
    googleOptions.CorrelationCookie.SameSite = SameSiteMode.None;
    googleOptions.CallbackPath = "/signin-google";



}).AddMicrosoftAccount(microsoftOptions =>
{
    microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
    microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];

});


builder.Services.AddRazorPages().AddRazorPagesOptions(o =>
{
    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});

builder.Services.AddControllers();
builder.Services.AddSingleton<INewsService, NewsService>();
builder.Services.AddSingleton<IWeatherService, WeatherService>();

builder.Services.AddHttpClient<WebServiceClient>();



builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;


});

builder.Services.Configure<WebServiceClientOptions>(configuration.GetSection("MaxMind"));

builder.Services.AddHttpClient();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;

});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Identity/Account/Login";
    options.SlidingExpiration = true;
});

builder.Services.ConfigureExternalCookie(options =>
{
    options.Cookie.Name = "ExternalCookie";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.Configure<SessionOptions>(options => { options.Cookie.SameSite = SameSiteMode.None; });

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "g_state";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Domain = "localhost";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();



app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

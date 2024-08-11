using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SixLabors.ImageSharp;
using System.Text;
using FluentValidation;
using WholeSaler.Web.MongoIdentity;
using WholeSaler.Web.Settings;
using FluentValidation.AspNetCore;
using WholeSaler.Web.FluentValidation.Configs;
using System.Net;
using Microsoft.AspNetCore.Authentication.Cookies;
using WholeSaler.Web.CustomMiddleWares;
using WholeSaler.Web.Hubs;
using WholeSaler.Web.Helpers.ProductHelper;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var mongoDbSettings = configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddMongoDbStores<AppUser, AppRole, Guid>(
    mongoDbSettings.ConnectionString, mongoDbSettings.Name).AddDefaultTokenProviders();




builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Program>());
builder.Services.AddSignalR();
builder.Services.AddScoped<IProductFilterService,ProductFilterService>();

builder.Services.AddTransient(typeof(IValidationService<>), typeof(ValidationService<>));

builder.Services.AddHttpClient();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie("WholeSalerCookie", x =>
    {
    x.LoginPath = "/user/Login";
        x.LogoutPath = "/home/index";
     
    x.AccessDeniedPath = "/user/Login";
    x.Cookie = new CookieBuilder
    {
        Name = "WholeSalerCookie"
    };
    x.SlidingExpiration = true;
    x.ExpireTimeSpan = TimeSpan.FromHours(24);
    x.Cookie.HttpOnly = true;
});

builder.Services.AddSession(options =>
{

    options.Cookie.Name = "WholeSaler.Session";
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();
app.UseMiddleware<TokenMiddleware>();
app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapHub<MessageHub>("/messagehub");
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "category",
    pattern: "Category/{categoryName}/{subCategoryName?}",
    defaults: new { controller = "Home", action = "category" });



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

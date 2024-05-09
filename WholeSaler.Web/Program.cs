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

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var mongoDbSettings = configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddMongoDbStores<AppUser, AppRole, Guid>(
    mongoDbSettings.ConnectionString, mongoDbSettings.Name).AddDefaultTokenProviders();


//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
  //.AddJwtBearer(options =>
  //  {
  //      options.TokenValidationParameters = new TokenValidationParameters
  //      {
  //          ValidateIssuer = true,
  //          ValidateAudience = true,
  //          ValidateLifetime = true,
  //          ValidateIssuerSigningKey = true,
  //          ValidIssuer = builder.Configuration["Jwt:Issuer"],
  //          ValidAudience = builder.Configuration["Jwt:Audience"],
  //          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
  //      };
  //  });
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddTransient(typeof(IValidationService<>), typeof(ValidationService<>));

builder.Services.AddHttpClient();


builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/user/Login";
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
app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

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

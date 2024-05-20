using Microsoft.AspNetCore.Authentication.JwtBearer;
using WholeSaler.IOC.Container;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WholeSaler.Api.Models.Jwt;
using WholeSaler.Api.AutoMapper.Mapping;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using WholeSaler.Business.TokenServices.Models;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using WholeSaler.Entity.Entities.MongoIdentity;
using Microsoft.Extensions.Configuration;
using WholeSaler.Api.MongoIdentity;
using Microsoft.AspNetCore.Identity;



var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
Log.Logger = new LoggerConfiguration()
    //.WriteTo.File("logs/info-.log", restrictedToMinimumLevel: LogEventLevel.Information)
    .WriteTo.File("logs/error-.log", restrictedToMinimumLevel: LogEventLevel.Error)
    .WriteTo.File("logs/warning-.log", restrictedToMinimumLevel: LogEventLevel.Warning)
    //.WriteTo.File("logs/debug-.log", restrictedToMinimumLevel: LogEventLevel.Debug)
    .CreateLogger();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder
            .WithOrigins("https://localhost:7189")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var mongoDbSettings = configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
builder.Services.AddIdentity<AppUser, AppRole>()
.AddMongoDbStores<AppUser, AppRole, Guid>(
    mongoDbSettings.ConnectionString, mongoDbSettings.Name).AddDefaultTokenProviders();




builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience,
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});



builder.Services.AddAutoMapper(typeof(MapProfile));
ConfigServices.ServiceConfiguration(builder.Services, builder.Configuration);
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders(); // Clear default logging providers
    loggingBuilder.AddSerilog(); // Add Serilog as a logging provider
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("MyCorsPolicy");

app.MapControllers();

app.Run();

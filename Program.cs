using ChatalkApi.Helpers;
using ChatalkApi.Models;
using ChatalkApi.Services.Concretes;
using ChatalkApi.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// EF Core for Db Context using Sqlite
builder.Services.AddDbContext<ChatalkDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ChatalkConnection")));

// AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Services
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors((policy) =>
{
    var appSettingsOptions = app.Services.GetService<IOptions<AppSettings>>();
    policy.WithOrigins(appSettingsOptions.Value.AllowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseMiddleware<JwtMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

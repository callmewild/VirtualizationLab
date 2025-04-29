using Microsoft.EntityFrameworkCore;
using VirtualizationLab1.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавляем конфигурацию БД
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "appdb";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "admin";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";

var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

var app = builder.Build();

// Применяем миграции при запуске
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapControllers();

app.Run();
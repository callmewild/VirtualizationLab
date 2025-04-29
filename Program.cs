using Microsoft.EntityFrameworkCore;
using VirtualizationLab.Data;

var builder = WebApplication.CreateBuilder(args);

// Явно указываем порт 80 для .NET 9
builder.WebHost.UseUrls("http://*:80");

// Конфигурация БД
var connectionString = builder.Configuration.GetConnectionString("Default") 
    ?? $"Host={Environment.GetEnvironmentVariable("DB_HOST") ?? "db"};"
     + $"Port={Environment.GetEnvironmentVariable("DB_PORT") ?? "5432"};"
     + $"Database={Environment.GetEnvironmentVariable("DB_NAME") ?? "appdb"};"
     + $"Username={Environment.GetEnvironmentVariable("DB_USER") ?? "admin"};"
     + $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password"}";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

var app = builder.Build();

// Автоматические миграции
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.MapControllers();

await app.RunAsync();
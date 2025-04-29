var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.WebHost.UseUrls("http://*:80");

app.MapGet("/", () => 
{
    var random = new Random();
    return $"Random number: {random.Next(1, 100)}. Current time: {DateTime.Now}";
});

app.Run();
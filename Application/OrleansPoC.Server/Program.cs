var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddKeyedRedisClient("redis");
builder.Host.UseOrleans(orleans => { orleans.UseDashboard(); });
var app = builder.Build();
app.MapGet("/", () => "OK");
app.Run();
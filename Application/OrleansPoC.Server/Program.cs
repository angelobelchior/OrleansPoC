var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddKeyedRedisClient("redis");
builder.Host.UseOrleans(orleans =>
{
    orleans.UseDashboard();
    if (builder.Environment.IsDevelopment())
        orleans.ConfigureEndpoints(Random.Shared.Next(10_000, 50_000), Random.Shared.Next(10_000, 50_000));
});
var app = builder.Build();
app.MapGet("/", () => "OK");
app.Run();
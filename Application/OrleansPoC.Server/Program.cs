var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", p => p
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.AddKeyedRedisClient("redis");
builder.UseOrleans(orleans =>
{
    orleans.UseDashboard();
    if (builder.Environment.IsDevelopment())
    {
        orleans.ConfigureEndpoints(Random.Shared.Next(10_000, 50_000), Random.Shared.Next(10_000, 50_000));
    }
});
var app = builder.Build();
app.UseCors("AllowAnyOrigin");
app.Run();
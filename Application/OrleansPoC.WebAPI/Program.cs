using OrleansPoC.WebAPI;
using OrleansPoC.WebAPI.Endpoints;

const string corsPolicyName = "AllowAnyOrigin";

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddKeyedRedisClient("redis");
builder.UseOrleansClient();

builder.Services.ConfigureOpenAPI();
builder.Services.ConfigureCors(corsPolicyName);

var app = builder.Build();
app.UseOpenAPI();
app.UseCors(corsPolicyName);

app.MapCustomers();
app.Run();
using OrleansPoC.Worker.MarketData;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.AddKeyedRedisClient("redis");
builder.UseOrleansClient();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
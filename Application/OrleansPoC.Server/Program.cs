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

/*
public class WebApiService(IHttpClientFactory httpClientFactory) : IDisposable
   {
       private readonly HttpClient _client = httpClientFactory.CreateClient();
       
       public async Task<Customer> CreateCustomerAsync(string name, string[] stocks)
       {
           var json = JsonSerializer.Serialize(new { name, stocks });
           var response = await _client.PostAsJsonAsync("customers", json);
           response.EnsureSuccessStatusCode();
   
           var content = await response.Content.ReadAsStringAsync();
           return JsonSerializer.Deserialize<Customer>(content) ?? throw new Exception("Failed to create customer");
       }
   
       public async Task<Customer> GetCustomerAsync(Guid id)
       {
           var content = await _client.GetStringAsync($"customers/{id}");
           return JsonSerializer.Deserialize<Customer>(content) ?? throw new Exception("Failed to create customer");
       }
   
       public async Task GetStocksAsync(Guid id, Action<Stock> onReceiveStock)
       {
           var stream = await _client.GetStreamAsync($"customers/{id}/stocks");
   
           using var reader = new StreamReader(stream);
           while (!reader.EndOfStream)
           {
               var line = await reader.ReadLineAsync();
               var parts = line?.Split(':');
               if (parts is null || parts.Length != 3) continue;
               onReceiveStock(new Stock
               {
                   Name = parts[1].Trim(),
                   Value = decimal.Parse(parts[2].Trim())
               });
           }
       }
   
       public void Dispose()
       {
           _client.Dispose();
       }
   }
*/
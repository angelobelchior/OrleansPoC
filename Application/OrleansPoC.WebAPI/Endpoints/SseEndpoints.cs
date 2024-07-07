using OrleansPoC.Contracts.Grains;

namespace OrleansPoC.WebAPI.Endpoints;

public static class SseEndpoints
{
    public static void MapSse(this WebApplication app)
    {
        app.MapGet("/sse/{id:guid}", async (
                IGrainFactory grains,
                Guid id,
                HttpContext context,
                CancellationToken cancellationToken)
            =>
        {
            context.Response.Headers.Append("Content-Type", "text/event-stream");

            var customerGrain = grains.GetGrain<ICustomerGrain>(id);
            if (customerGrain is null) return Results.NotFound();

            var customer = await customerGrain.Get();
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var stock in customer.Stocks)
                {
                    var stockGrain = grains.GetGrain<IStockGrain>(stock.Name);
                    var stocksItems = await stockGrain.Get(stock.Name);
                    foreach (var stockItem in stocksItems)
                    {
                        await context.Response.WriteAsync($"data: {stockItem.Name}:{stockItem.Value}\n\n",
                            cancellationToken);
                        await context.Response.Body.FlushAsync(cancellationToken);
                    }
                }
            }
            
            return Results.Empty;
        });
    }
}
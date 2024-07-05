using Orleans.Runtime;
using Orleans.Streams;
using OrleansPoC.Contracts.Models;

namespace OrleansPoC.WebAPI.Endpoints;

public static class SSEEndpoints
{
    public static void MapSSE(this WebApplication app)
    {
        app.MapGet("/sse", async (
                IClusterClient client,
                HttpContext context,
                CancellationToken cancellationToken)
            =>
        {
            var streamProvider = client.GetStreamProvider("streaming-provider");
            var streamId = StreamId.Create("Stock", "Updated");
            var stream = streamProvider.GetStream<Stock>(streamId);
            var subscription = await stream.SubscribeAsync(async (stock, _) =>
            {
                await context.Response.WriteAsync($"data: {stock.Name}:{stock.Value}\n\n", cancellationToken);
                await context.Response.Body.FlushAsync(cancellationToken);
            });
            
            context.Response.Headers.Append("Content-Type", "text/event-stream");
            while (cancellationToken.IsCancellationRequested) 
                await subscription.UnsubscribeAsync();
        });
    }
}
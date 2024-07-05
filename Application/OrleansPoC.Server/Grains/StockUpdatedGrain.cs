using Orleans.Runtime;
using Orleans.Streams;
using OrleansPoC.Contracts.Grains;
using OrleansPoC.Contracts.Models;

namespace OrleansPoC.Server.Grains;

public class StockUpdatedGrain(ILogger<StockUpdatedGrain> logger) 
    : Grain, IStockUpdatedGrain
{
    private IAsyncStream<Stock> _stream = null!;
    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        try
        {
            var streamProvider = this.GetStreamProvider("streaming-provider");
            var streamId = StreamId.Create("Stock", "Updated");
            _stream = streamProvider.GetStream<Stock>(streamId);
        }
        catch (Exception e)
        {
            logger.LogError(e, "{Message}", e.Message);
        }
        
        return base.OnActivateAsync(cancellationToken);
    }

    public async ValueTask Send(decimal value)
    {
        var stock = new Stock
        {
            Name = this.GetPrimaryKeyString(),
            Value = value
        };
        
        logger.LogInformation("Server.Send[{Datetime}] => {Stock}:{Value}", DateTime.Now, stock.Name, stock.Value);
        await _stream.OnNextAsync(stock);
    }
}
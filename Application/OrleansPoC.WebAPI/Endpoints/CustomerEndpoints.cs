using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using OrleansPoC.Contracts.Grains;
using OrleansPoC.Contracts.Models;

namespace OrleansPoC.WebAPI.Endpoints;

public static class CustomerEndpoints
{
    private static readonly Lock _lockObject = new();

    public static void MapCustomers(this WebApplication app)
    {
        app.MapPost("/customers", async (
                IGrainFactory grainFactory,
                [FromBody] Models.Customer customer)
            =>
        {
            if (!MiniValidator.TryValidate(customer, out var errors))
                return Results.ValidationProblem(errors);

            var id = Guid.NewGuid();
            var customerGrain = grainFactory.GetGrain<ICustomerGrain>(id);
            var newCustomer = await customerGrain.Create(customer.Name, customer.Stocks);
            return Results.Created($"/customers/{newCustomer.Id}/stocks", newCustomer);
        });

        app.MapGet("/customers/{id:guid}", async (
                IGrainFactory grainFactory,
                Guid id)
            =>
        {
            var grain = grainFactory.GetGrain<ICustomerGrain>(id);
            var customer = await grain.Get();
            return Results.Ok(customer);
        });

        app.MapGet("customers/{id:Guid}/stocks", async (
                IClusterClient clusterClient,
                Guid id,
                HttpContext context,
                CancellationToken cancellationToken)
            =>
        {
            var observersList =
                new List<(IStockGrain StockGrain, IConsumerObserver ConsumerObserver)>();

            try
            {
                var customerGrain = clusterClient.GetGrain<ICustomerGrain>(id);

                var customer = await customerGrain.Get();
                if (customer is null) return Results.NotFound();

                context.Response.Headers.Append("Content-Type", "text/event-stream");
                foreach (var stock in customer.Stocks)
                {
                    var stocksObserverGrain = clusterClient.GetGrain<IStockGrain>(stock);

                    var observer = new ConsumerObserver(context.Response, cancellationToken);
                    var observerRef = clusterClient.CreateObjectReference<IConsumerObserver>(observer);

                    observersList.Add((stocksObserverGrain, observerRef));

                    lock (_lockObject)
                        stocksObserverGrain.Subscribe(observerRef);
                }

                while (!cancellationToken.IsCancellationRequested)
                {
                    // Keep the connection open
                }

                return Results.Empty;
            }
            catch
            {
                lock (_lockObject)
                {
                    foreach (var (stocksGrain, consumerObserver) in observersList)
                        stocksGrain.Unsubscribe(consumerObserver);
                }
                return Results.Empty;
            }
        });
    }

    private class ConsumerObserver(HttpResponse response, CancellationToken cancellationToken) : IConsumerObserver
    {
        public async Task OnStockUpdated(Stock stock)
        {
            await response.WriteAsync("event: stockChanged", cancellationToken: cancellationToken);
            await response.WriteAsync("\n", cancellationToken: cancellationToken);
            await response.WriteAsync("data: ", cancellationToken: cancellationToken);
            await JsonSerializer.SerializeAsync(response.Body, stock, cancellationToken: cancellationToken);
            await response.WriteAsync("\n\n", cancellationToken: cancellationToken);
            await response.Body.FlushAsync(cancellationToken);
        }
    }
}
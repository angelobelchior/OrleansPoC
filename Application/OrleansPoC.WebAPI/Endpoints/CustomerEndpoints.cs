using Microsoft.AspNetCore.Mvc;
using MiniValidation;
using OrleansPoC.Contracts.Grains;

namespace OrleansPoC.WebAPI.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomers(this WebApplication app)
    {
        app.MapPost("/customers", async (
                IGrainFactory grains,
                [FromBody] Models.Customer customer)
            =>
        {
            if (!MiniValidator.TryValidate(customer, out var errors))
                return Results.ValidationProblem(errors);

            var id = Guid.NewGuid();
            var customerGrain = grains.GetGrain<ICustomerGrain>(id);
            var newCustomer = await customerGrain.Create(customer.Name, customer.Stocks);
            return Results.Created($"/customers/{newCustomer.Id}", newCustomer);
        });

        app.MapGet("/customers/{id:guid}", async (
                IGrainFactory grains,
                Guid id)
            =>
        {
            var grain = grains.GetGrain<ICustomerGrain>(id);
            var customer = await grain.Get();
            return Results.Ok(customer);
        });
        
        app.MapGet("customers/{id:guid}/stocks", async (
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
                    var stockGrain = grains.GetGrain<IStockGrain>(stock);
                    var stockItem = await stockGrain.Get();
                    await context.Response.WriteAsync($"data: {stockItem.Name}:{stockItem.Value}\n\n",
                        cancellationToken);
                    await context.Response.Body.FlushAsync(cancellationToken);
                }
            }

            return Results.Empty;
        });
    }
}
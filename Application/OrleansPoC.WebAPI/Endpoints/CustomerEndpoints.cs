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
    }
}
namespace OrleansPoC.WebAPI;

public static class Extensions
{
    public static void ConfigureCors(this IServiceCollection services, string corsPolicyName)
    {
        services
            .AddCors(options
                => options.AddPolicy(
                    name: corsPolicyName,
                    configurePolicy: policyBuilder =>
                    {
                        policyBuilder.AllowAnyHeader();
                        policyBuilder.AllowAnyMethod();
                        policyBuilder.AllowAnyOrigin();
                    }));
    }
    
    public static void ConfigureOpenAPI(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
    
    public static void UseOpenAPI(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
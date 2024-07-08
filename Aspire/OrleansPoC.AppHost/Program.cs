var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis", 6379)
                   ;

var orleans = builder.AddOrleans("default")
        .WithClustering(redis)
        .WithMemoryGrainStorage("stocks")
        .WithGrainStorage("customers", redis)
        ;

builder.AddProject<Projects.OrleansPoC_Server>("silo")
        .WithReference(orleans)
        .WithReplicas(3)
        ;

var webApi = builder.AddProject<Projects.OrleansPoC_WebAPI>("web-api")
        .WithReference(orleans.AsClient())
        .WithExternalHttpEndpoints()
        ;

builder.AddProject<Projects.OrleansPoC_UI>("ui")        
        .WithReference(webApi)
        .WithExternalHttpEndpoints()
        ;

builder.AddProject<Projects.OrleansPoC_Worker_MarketData>("market-data")
        .WithReference(orleans.AsClient())
        ;

builder.Build().Run();
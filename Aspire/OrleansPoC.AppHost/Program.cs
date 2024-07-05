var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis")
                   ;

var orleans = builder.AddOrleans("default")
        .WithClustering(redis)
        .WithMemoryStreaming("streaming-provider")
        .WithMemoryGrainStorage("storage")
        ;

builder.AddProject<Projects.OrleansPoC_Server>("silo")
        .WithReference(orleans)
        .WithReplicas(3)
        ;

builder.AddProject<Projects.OrleansPoC_WebAPI>("web-api")
        .WithReference(orleans.AsClient())
        .WithExternalHttpEndpoints();

builder.AddProject<Projects.OrleansPoC_Worker_MarketData>("market-data")
        .WithReference(orleans.AsClient())
        ;

builder.Build().Run();
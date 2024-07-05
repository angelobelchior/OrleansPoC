var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");

var orleans = builder.AddOrleans("default")
        .WithClustering(redis)
        .WithMemoryStreaming("orleans-streaming-provider")
        .WithMemoryGrainStorage("orleans-storage")
    ;

var server = builder.AddProject<Projects.OrleansPoC_Server>("server")
        .WithReference(orleans)
        .WithReplicas(3)
    ;

var marketData = builder.AddProject<Projects.OrleansPoC_Worker_MarketData>("market-data")
        .WithReference(orleans.AsClient())
    ;

builder.Build().Run();
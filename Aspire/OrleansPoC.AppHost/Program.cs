var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis", 6379)
                   .WithRedisInsight()
                   ;

var orleans = builder.AddOrleans("default")
        .WithClustering(redis)
        .WithMemoryGrainStorage("stocks")
        .WithMemoryGrainStorage("customers")
        //.WithGrainStorage("customers", redis)
        ;

builder.AddProject<Projects.OrleansPoC_Server>("silo")
        .WithReplicas(3)
        .WithReference(orleans)
        ;

var webApi = builder.AddProject<Projects.OrleansPoC_WebAPI>("web-api")
        .WithReference(orleans.AsClient())
        .WithExternalHttpEndpoints()
        ;

builder.AddProject<Projects.OrleansPoC_Worker_MarketData>("market-data")
        .WithReference(orleans.AsClient())
        ;

builder.Build().Run();
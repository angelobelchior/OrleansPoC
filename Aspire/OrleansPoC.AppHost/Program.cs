var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis", 6379)
        .WithRedisInsight(config =>
        {
            config.WithHostPort(6378);
        })
    ;

var orleans = builder.AddOrleans("default")
        .WithClustering(redis)
        .WithMemoryGrainStorage("stocks")
        .WithGrainStorage("customers", redis)
    ;

var silo = builder.AddProject<Projects.OrleansPoC_Server>("cluster")
        .WaitFor(redis)
        .WithReplicas(1)
        .WithReference(orleans)
    ;

var webApi = builder.AddProject<Projects.OrleansPoC_WebAPI>("web-api")
        .WaitFor(silo)
        .WaitFor(redis)
        .WithReference(orleans.AsClient())
        .WithExternalHttpEndpoints()
    ;

var marketData = builder.AddProject<Projects.OrleansPoC_Worker_MarketData>("market-data")
        .WaitFor(silo)
        .WaitFor(webApi)
        .WithReference(orleans.AsClient())
    ;

builder.Build().Run();
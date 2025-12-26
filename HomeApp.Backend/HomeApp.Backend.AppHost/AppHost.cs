var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("homeappContainer")
    .WithHostPort(5060)
    .WithDataVolume();

var homeAppDb = postgres.AddDatabase(
    name: "HomeAppConnection",
    databaseName: "homeapp"
);

var api = builder.AddProject<Projects.Web_Api>("api")
    .WithReference(homeAppDb)
    .WaitFor(homeAppDb)
    .WithHttpHealthCheck("/health/ready");

builder.Build().Run();

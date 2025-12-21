var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Web_Api>("api")
    .WithHttpHealthCheck("/health");

builder.Build().Run();

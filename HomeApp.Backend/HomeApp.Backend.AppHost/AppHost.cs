var builder = DistributedApplication.CreateBuilder(args);

var keycloakAdminPassword = builder.AddParameter("keycloak-admin-password", value: "admin", secret: true);
var bffClientSecret = builder.AddParameter("bff-client-secret", value: "local-homeapp-bff-secret", secret: true);

var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "26.6.3")
    .WithHttpEndpoint(port: 8080, targetPort: 8080, name: "http")
    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", keycloakAdminPassword)
    .WithEnvironment("KC_HTTP_ENABLED", "true")
    .WithEnvironment("KC_HOSTNAME_STRICT", "false")
    .WithEnvironment("KC_HOSTNAME_STRICT_HTTPS", "false")
    .WithArgs("start-dev", "--import-realm")
    .WithBindMount("./keycloak/realm-export.json", "/opt/keycloak/data/import/realm-export.json");

var postgres = builder
    .AddPostgres("homeappContainer")
    .WithHostPort(5060)
    .WithDataVolume();

var homeAppDb = postgres.AddDatabase(
    name: "HomeAppConnection",
    databaseName: "homeapp");

var api = builder.AddProject<Projects.Web_Api>("api")
    .WithReference(homeAppDb)
    .WaitFor(homeAppDb)
    .WaitFor(keycloak)
    .WithEnvironment("OAuth__Authority", "http://localhost:8080/realms/homeapp")
    .WithEnvironment("OAuth__ValidAudiences__0", "local-homeapp-api")
    .WithHttpHealthCheck("/health/ready");

builder.AddProject<Projects.HomeApp_Bff>("bff")
    .WithReference(api)
    .WaitFor(keycloak)
    .WaitFor(api)
    .WithEnvironment("ASPNETCORE_URLS", "http://+:5000")
    .WithEnvironment("OAuth__Authority", "http://localhost:8080/realms/homeapp")
    .WithEnvironment("OAuth__ClientId", "local-homeapp-bff")
    .WithEnvironment("OAuth__ClientSecret", bffClientSecret)
    .WithEnvironment("ReverseProxy__Clusters__api-cluster__Destinations__api__Address",
        ReferenceExpression.Create($"{api.GetEndpoint("http")}/"))
    .WithEndpoint("http", endpoint =>
    {
        endpoint.Port = 5000;
        endpoint.IsProxied = false;
    });

builder.Build().Run();

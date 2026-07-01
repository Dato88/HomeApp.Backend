var builder = DistributedApplication.CreateBuilder(args);

var keycloakAdminPassword = builder.AddParameter("keycloak-admin-password", value: "admin", secret: true);
var bffClientSecret =
    builder.AddParameter("bff-client-secret", value: "ZJHzn6jImwN7sjvIMdB3SxRQDMN8Z56N", secret: true);

var keycloakPostgres = builder
    .AddPostgres("keycloakContainer")
    .WithDataVolume();

var keycloakDb = keycloakPostgres.AddDatabase(
    name: "KeycloakConnection",
    databaseName: "keycloak");

var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "26.6.3")
    .WithHttpEndpoint(port: 8080, targetPort: 8080, name: "http")
    .WithReference(keycloakDb)
    .WaitFor(keycloakDb)
    .WithEnvironment("KC_BOOTSTRAP_ADMIN_USERNAME", "admin")
    .WithEnvironment("KC_BOOTSTRAP_ADMIN_PASSWORD", keycloakAdminPassword)
    .WithEnvironment("KC_HTTP_ENABLED", "true")
    .WithEnvironment("KC_HOSTNAME_STRICT", "false")
    .WithEnvironment("KC_HOSTNAME_STRICT_HTTPS", "false")
    .WithEnvironment("KC_DB", "postgres")
    .WithEnvironment("KC_DB_URL", keycloakDb.Resource.JdbcConnectionString)
    .WithEnvironment("KC_DB_USERNAME", keycloakPostgres.Resource.UserNameReference)
    .WithEnvironment("KC_DB_PASSWORD", keycloakPostgres.Resource.PasswordParameter)
    .WithBindMount("./keycloak", "/opt/keycloak/data/import")
    .WithArgs("start", "--import-realm");

var postgres = builder
    .AddPostgres("homeappContainer")
    .WithHostPort(5060)
    .WithDataVolume();

var homeAppDb = postgres.AddDatabase(
    name: "HomeAppConnection",
    databaseName: "homeapp");

var api = builder.AddProject<Projects.Web_Api>("api", launchProfileName: null)
    .WithReference(homeAppDb)
    .WaitFor(homeAppDb)
    .WaitFor(keycloak)
    .WithEnvironment("OAuth__Authority", "http://localhost:8080/realms/homeapp")
    .WithEnvironment("OAuth__ValidAudiences__0", "local-homeapp-api")
    .WithEndpoint("https", endpoint =>
    {
        endpoint.Port = 7254;
        endpoint.IsProxied = false;
    });

builder.AddProject<Projects.HomeApp_Bff>("bff", launchProfileName: null)
    .WithReference(api)
    .WaitFor(keycloak)
    .WaitFor(api)
    .WithEnvironment("OAuth__Authority", "http://localhost:8080/realms/homeapp")
    .WithEnvironment("OAuth__ClientId", "local-homeapp-bff")
    .WithEnvironment("OAuth__ClientSecret", bffClientSecret)
    .WithEnvironment("OAuth__RedirectUri", "http://localhost:4200/auth/callback")
    .WithEnvironment("OAuth__PostLogoutRedirectUri", "http://localhost:4200")
    .WithEnvironment("OAuth__PostLoginRedirectUri", "http://localhost:4200")
    .WithEnvironment("OAuth__AngularOrigin", "http://localhost:4200")
    .WithEnvironment("ReverseProxy__Clusters__api-cluster__Destinations__api__Address", "https://localhost:7254/")
    .WithHttpEndpoint(port: 5555, isProxied: false);

builder.Build().Run();

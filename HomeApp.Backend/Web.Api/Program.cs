using Application;
using Infrastructure;
using Web.Api;
using Web.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddPresentation();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseScalarApiWithUi();

app.UseAuthenticationExtension();

app.MapControllers().RequireCors("CorsPolicy");

await app.RunAsync();

public partial class Program
{
}

using System.Security.Claims;
using HomeApp.DataAccess.Models;
using HomeApp.Identity.Models;
using HomeApp.Identity.Utilities;
using HomeApp.Library.Cruds;
using HomeApp.Library.Cruds.Interfaces;
using HomeApp.Library.Facades;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Validation.Interfaces;
using HomeApp.Library.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ILogger = Serilog.ILogger;
using User = HomeApp.Identity.Models.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

ILogger logger =
    new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();

builder.Services.AddSingleton(logger);

builder.Services.AddDbContext<HomeAppContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("HomeAppConnection"));
});

builder.Services.AddDbContext<UserContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("HomeAppUserConnection"));
});

builder.Services.AddScoped<IBudgetValidation, BudgetValidation>();
builder.Services.AddScoped<IUserValidation, UserValidation>();

builder.Services.AddScoped<IBudgetCellCrud, BudgetCellCrud>();
builder.Services.AddScoped<IBudgetColumnCrud, BudgetColumnCrud>();
builder.Services.AddScoped<IBudgetGroupCrud, BudgetGroupCrud>();
builder.Services.AddScoped<IBudgetRowCrud, BudgetRowCrud>();
builder.Services.AddScoped<IBudgetFacade, BudgetFacade>();

builder.Services.AddAuthorization(options =>
{
    foreach (var item in ClaimStore.AllClaims)
    {
        options.AddPolicy($"{item.Type.Replace(" ", "")}Policy", policy => policy.RequireClaim(item.Value));
    }
});
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<UserContext>()
    .AddApiEndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
using HomeApp.DataAccess.Models;
using HomeApp.Identity.Cruds;
using HomeApp.Identity.Cruds.Interfaces;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy => policy
        .WithOrigins("http://localhost:4200", "http://localhost:4200", "https://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod());
});


builder.Services.AddScoped<IBudgetValidation, BudgetValidation>();
builder.Services.AddScoped<IUserValidation, UserValidation>();

builder.Services.AddScoped<IBudgetCellCrud, BudgetCellCrud>();
builder.Services.AddScoped<IBudgetColumnCrud, BudgetColumnCrud>();
builder.Services.AddScoped<IBudgetGroupCrud, BudgetGroupCrud>();
builder.Services.AddScoped<IBudgetRowCrud, BudgetRowCrud>();
builder.Services.AddScoped<IBudgetFacade, BudgetFacade>();

builder.Services.AddScoped<IUserCrud, UserCrud>();

builder.Services.AddAuthorization(options =>
{
    foreach (var item in ClaimStore.AllClaims)
    {
        options.AddPolicy($"{item.Type.Replace(" ", "")}Policy", policy => policy.RequireClaim(item.Value));
    }
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<UserContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers().RequireCors("CorsPolicy");

app.Run();
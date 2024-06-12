using HomeApp.DataAccess.Models;
using HomeApp.Library.Cruds;
using HomeApp.Library.Cruds.Interfaces;
using HomeApp.Library.Facades;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Validation.Interfaces;
using HomeApp.Library.Validations;
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

builder.Services.AddScoped<IBudgetValidation, BudgetValidation>();
builder.Services.AddScoped<IUserValidation, UserValidation>();

builder.Services.AddScoped<IBudgetCellCrud, BudgetCellCrud>();
builder.Services.AddScoped<IBudgetColumnCrud, BudgetColumnCrud>();
builder.Services.AddScoped<IBudgetGroupCrud, BudgetGroupCrud>();
builder.Services.AddScoped<IBudgetRowCrud, BudgetRowCrud>();
builder.Services.AddScoped<IBudgetFacade, BudgetFacade>();

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

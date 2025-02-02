using System.Text;
using HomeApp.DataAccess.Cruds;
using HomeApp.DataAccess.Cruds.Interfaces;
using HomeApp.DataAccess.Cruds.Interfaces.People;
using HomeApp.DataAccess.Cruds.Interfaces.Todos;
using HomeApp.DataAccess.Cruds.People;
using HomeApp.DataAccess.Cruds.Todos;
using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Validations;
using HomeApp.DataAccess.Validations.Interfaces;
using HomeApp.Identity.Cruds;
using HomeApp.Identity.Cruds.Interfaces;
using HomeApp.Identity.Entities.Models;
using HomeApp.Identity.Handler;
using HomeApp.Identity.Utilities;
using HomeApp.Library;
using HomeApp.Library.Email;
using HomeApp.Library.Facades;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Models.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    options.UseNpgsql(builder.Configuration.GetConnectionString("HomeAppConnection")));

builder.Services.AddDbContext<UserContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("HomeAppUserConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy => policy
        .WithOrigins("http://localhost:4200", "https://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddScoped<IBudgetValidation, BudgetValidation>();
builder.Services.AddScoped<IPersonValidation, PersonValidation>();

builder.Services.AddScoped<IBudgetCellCrud, BudgetCellQueries>();
builder.Services.AddScoped<IBudgetColumnCrud, BudgetColumnQueries>();
builder.Services.AddScoped<IBudgetGroupCrud, BudgetGroupQueries>();
builder.Services.AddScoped<IBudgetRowCrud, BudgetRowQueries>();
builder.Services.AddScoped<IBudgetFacade, BudgetFacade>();
builder.Services.AddScoped<IPersonCommands, PersonCommands>();
builder.Services.AddScoped<IPersonQueries, PersonQueries>();
builder.Services.AddScoped<IPersonFacade, PersonFacade>();
builder.Services.AddScoped<ITodoCommands, TodoCommands>();
builder.Services.AddScoped<ITodoQueries, TodoQueries>();
builder.Services.AddScoped<ITodoGroupCrud, TodoGroupQueries>();
builder.Services.AddScoped<ITodoGroupTodoCrud, TodoGroupTodoQueries>();
builder.Services.AddScoped<ITodoPersonCrud, TodoPersonQueries>();

builder.Services.AddScoped<JwtHandler>();

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddScoped<IUserCrud, UserCrud>();

builder.Services.AddAuthorization(options =>
{
    foreach (var item in ClaimStore.AllClaims)
        options.AddPolicy($"{item.Type.Replace(" ", "")}Policy", policy => policy.RequireClaim(item.Value));
});

builder.Services.AddIdentity<User, IdentityRole>(
        opt =>
        {
            opt.Password.RequiredLength = 7;
            opt.Password.RequireDigit = false;

            opt.User.RequireUniqueEmail = true;

            opt.Lockout.AllowedForNewUsers = true;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            opt.Lockout.MaxFailedAccessAttempts = 3;
        })
    .AddEntityFrameworkStores<UserContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromHours(1));

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings.GetSection("securityKey").Value))
    };
});

builder.Services.AddHttpContextAccessor();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireCors("CorsPolicy");

app.Run();

public partial class Program
{
}

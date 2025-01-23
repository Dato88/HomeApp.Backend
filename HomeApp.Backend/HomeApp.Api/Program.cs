using System.Text;
using HomeApp.DataAccess.Models;
using HomeApp.Identity.Cruds;
using HomeApp.Identity.Cruds.Interfaces;
using HomeApp.Identity.Entities.Models;
using HomeApp.Identity.Handler;
using HomeApp.Identity.Utilities;
using HomeApp.Library.Cruds;
using HomeApp.Library.Cruds.Interfaces;
using HomeApp.Library.Email;
using HomeApp.Library.Facades;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Models.Email;
using HomeApp.Library.Validations;
using HomeApp.Library.Validations.Interfaces;
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

builder.Services.AddScoped<IBudgetCellCrud, BudgetCellCrud>();
builder.Services.AddScoped<IBudgetColumnCrud, BudgetColumnCrud>();
builder.Services.AddScoped<IBudgetGroupCrud, BudgetGroupCrud>();
builder.Services.AddScoped<IBudgetRowCrud, BudgetRowCrud>();
builder.Services.AddScoped<IBudgetFacade, BudgetFacade>();
builder.Services.AddScoped<IPersonCrud, PersonCrud>();
builder.Services.AddScoped<IPersonFacade, PersonFacade>();
builder.Services.AddScoped<ITodoCrud, TodoCrud>();
builder.Services.AddScoped<ITodoGroupCrud, TodoGroupCrud>();
builder.Services.AddScoped<ITodoGroupTodoCrud, TodoGroupTodoCrud>();
builder.Services.AddScoped<ITodoPersonCrud, TodoPersonCrud>();
builder.Services.AddScoped<ITodoFacade, TodoFacade>();

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

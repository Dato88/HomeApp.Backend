using Application.Common.Interfaces.People;
using Application.Common.Interfaces.Todos;
using Application.Common.People;
using Application.Common.People.Validations;
using Application.Common.People.Validations.Interfaces;
using Application.Common.Todos;
using Application.Email;
using Application.Models.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddMediatR()
            .AddServices(configuration);

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddScoped<IBudgetValidation, BudgetValidation>();
        services.AddScoped<IPersonValidation, PersonValidation>();

        // services.AddScoped<IBudgetCellCrud, BudgetCellQueries>();
        // services.AddScoped<IBudgetColumnCrud, BudgetColumnQueries>();
        // services.AddScoped<IBudgetGroupCrud, BudgetGroupQueries>();
        // builder.Services.AddScoped<IBudgetRowCrud, BudgetRowQueries>();
        // builder.Services.AddScoped<IBudgetFacade, BudgetFacade>();
        services.AddScoped<ICommonPersonCommands, CommonPersonCommands>();
        services.AddScoped<ICommonPersonQueries, CommonPersonQueries>();
        services.AddScoped<ITodoCommands, TodoCommands>();
        services.AddScoped<ITodoQueries, TodoQueries>();

        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        services.AddSingleton(emailConfig);
        services.AddScoped<IEmailSender, EmailSender>();

        return services;
    }
}

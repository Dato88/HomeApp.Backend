using Application.Configurations;
using Application.Email;
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

        // services.AddScoped<IBudgetCellCrud, BudgetCellQueries>();
        // services.AddScoped<IBudgetColumnCrud, BudgetColumnQueries>();
        // services.AddScoped<IBudgetGroupCrud, BudgetGroupQueries>();
        // builder.Services.AddScoped<IBudgetRowCrud, BudgetRowQueries>();
        // builder.Services.AddScoped<IBudgetFacade, BudgetFacade>();


        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        services.AddSingleton(emailConfig);
        services.AddScoped<IEmailSender, EmailSender>();

        return services;
    }
}

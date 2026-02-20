using ExpenseTracker.Application;
using ExpenseTracker.Application.Services.ExpenseServices;
using ExpenseTracker.Infrastructure;

namespace ExpenseTrackerApi;

public static class DependencyInjection
{
    public static IServiceCollection AddMainApiDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationDI()
            .AddInfrastructureDI(configuration);

        services.AddScoped<ExpenseService>(); // Register ExpenseService (use case)


        return services;
    }

}

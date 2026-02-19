using ExpenseTracker.Application;
using ExpenseTracker.Infrastructure;

namespace ExpenseTrackerApi;

public static class DependencyInjection
{
    public static IServiceCollection AddMainApiDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationDI()
            .AddInfrastructureDI(configuration);


        return services;
    }

}

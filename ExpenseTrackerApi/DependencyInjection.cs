using ExpenseTracker.Application;
using ExpenseTracker.Infrastructure;

namespace ExpenseTrackerApi;

public static class DependencyInjection
{
    public static IServiceCollection AddMainApiDI(this IServiceCollection services)
    {
        services.AddApplicationDI()
            .AddInfrastructureDI();


        return services;
    }

}

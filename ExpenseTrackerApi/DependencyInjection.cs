using ExpenseTracker.Application;
using ExpenseTracker.Application.Services.CategoryServices;
using ExpenseTracker.Application.Services.ExpenseServices;
using ExpenseTracker.Application.Services.UserServices;
using ExpenseTracker.Infrastructure;

namespace ExpenseTrackerApi;

public static class DependencyInjection
{
    public static IServiceCollection AddMainApiDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationDI()
            .AddInfrastructureDI(configuration);

        services.AddScoped<ExpenseService>(); // Register ExpenseService (use case)
        services.AddScoped<UserService>(); // Register UserService (use case)
        services.AddScoped<CategoryService>(); // Register CategoryService (use case)


        return services;
    }

}

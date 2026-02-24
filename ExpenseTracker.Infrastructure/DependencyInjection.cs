using ExpenseTracker.Infrastructure.DataBase;
using ExpenseTracker.Infrastructure.Persistence.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Infrastructure.Repositories;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Security;
using Microsoft.AspNetCore.Identity;
using ExpenseTracker.Infrastructure.Repositories.Security;
using ExpenseTracker.Infrastructure.Background;
using ExpenseTracker.Application.Interfaces.Caching;
using ExpenseTracker.Infrastructure.Caching;


namespace ExpenseTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DataContext>( options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        // registered redis cache services with the configuration from appsettings.json. The instance name is set to "ExpenseTracker:" to namespace the cache keys and avoid conflicts with other applications that might be using the same Redis instance.
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:Configuration"];
            options.InstanceName = "ExpenseTracker:";
        });

        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IPasswordHasher, AppPasswordHasher>();

        services.AddHostedService<BudgetAlertWorker>(); // Register the background worker

        services.AddScoped<ICacheService, RedisCacheService>(); //register the cache service to be used in the application, allowing for caching of data using Redis as the underlying cache store.

        return services;
    }
}

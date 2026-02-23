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

        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IPasswordHasher, AppPasswordHasher>();

        services.AddHostedService<BudgetAlertWorker>(); // Register the background worker

        return services;
    }
}

using ExpenseTracker.Infrastructure.DataBase;
using ExpenseTracker.Infrastructure.Persistence.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;


namespace ExpenseTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DataContext>((provider, options) =>
        {
            options.UseNpgsql(provider.GetRequiredService<IOptionsSnapshot<ConnectionStringOptions>>().Value.DefaultConnection);
        });
        return services;
    }
}

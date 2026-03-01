using ExpenseTracker.Application.DTOs.EmailSender;
using ExpenseTracker.Application.Interfaces.Caching;
using ExpenseTracker.Application.Interfaces.Email;
using ExpenseTracker.Application.Interfaces.JwtSettings;
using ExpenseTracker.Application.Interfaces.Reports;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Security;
using ExpenseTracker.Application.Settings;
using ExpenseTracker.Infrastructure.Background;
using ExpenseTracker.Infrastructure.Caching;
using ExpenseTracker.Infrastructure.DataBase;
using ExpenseTracker.Infrastructure.Persistence.Options;
using ExpenseTracker.Infrastructure.Repositories;
using ExpenseTracker.Infrastructure.Repositories.EmailSender;
using ExpenseTracker.Infrastructure.Repositories.Reports;
using ExpenseTracker.Infrastructure.Repositories.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


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
            options.Configuration = configuration["Redis:ConnectionString"];
            options.InstanceName = configuration["Redis:InstanceName"];
        });

        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IPasswordHasher, AppPasswordHasher>();

        services.AddHostedService<BudgetAlertWorker>(); // Register the background worker
        services.AddHostedService<ReportJobWorker>(); // Register the background worker for processing report jobs

        services.AddScoped<ICacheService, RedisCacheService>(); //register the cache service to be used in the application, allowing for caching of data using Redis as the underlying cache store.

        services.AddScoped<IReportService, ReportService>(); // Register the report service to handle report generation and management.


        // registered the email settings and email service to handle email sending functionality in the application
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, EmailService>();

        //jwt registration
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}

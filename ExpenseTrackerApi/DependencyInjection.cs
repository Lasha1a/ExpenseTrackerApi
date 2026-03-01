using ExpenseTracker.Application;
using ExpenseTracker.Application.Services.CategoryServices;
using ExpenseTracker.Application.Services.ExpenseServices;
using ExpenseTracker.Application.Services.UserServices;
using ExpenseTracker.Application.Settings;
using ExpenseTracker.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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


        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
        });


        return services;
    }

}

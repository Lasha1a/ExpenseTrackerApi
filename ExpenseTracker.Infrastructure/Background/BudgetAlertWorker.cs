using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Specifications;
using ExpenseTracker.Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Background;

public class BudgetAlertWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public BudgetAlertWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }



    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //run as long as the application is running
        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckBudgetsAsync(stoppingToken);

            //run every 10 mins
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }

    public async Task CheckBudgetsAsync(CancellationToken stoppingToken)
    {
        //create a new scope to resolve scoped services
        using var scope = _scopeFactory.CreateScope();

        //resolve the repositories from the service provider
        var categoryRepo = scope.ServiceProvider.GetRequiredService<IRepository<Category>>;
        var expenseRepo = scope.ServiceProvider.GetRequiredService<IRepository<Expense>>();
        var alertRepo = scope.ServiceProvider.GetRequiredService<IRepository<BudgetAlert>>();

        //get all categories
        var now = DateTime.UtcNow;
        var year = now.Year;
        var month = now.Month;

        //get all categories and their budgets w active status
        var categories = await categoryRepo
                .ListAsync(new ActiveCategoriesWithBudgetSpec());

        foreach (var category in categories)
        {
            // Calculate how much was spent THIS MONTH for this category
            var expenses = await expenseRepo.ListAsync(
                new ExpensesByCategoryAndMonthSpec(
                    category.UserId,
                    category.Id,
                    year,
                    month
                )
            );

            var spent = expenses.Sum(e => e.Amount);

            if (category.MonthlyBudget <= 0)
                continue;

            var percentageUsed = (spent / category.MonthlyBudget) * 100;

            // Only trigger alert if >= 80%
            if (percentageUsed < 80)
                continue;

            // Check if alert already exists for this month
            var existingAlert = await alertRepo.FirstOrDefaultAsync(
                new BudgetAlertByCategoryMonthSpec(
                    category.UserId,
                    category.Id,
                    year,
                    month
                )
            );

            if (existingAlert != null)
                continue; // Prevent duplicate alerts

            // Create new alert
            var alert = new BudgetAlert
            {
                Id = Guid.NewGuid(),
                UserId = category.UserId,
                CategoryId = category.Id,
                Year = year,
                Month = month,
                PercentageUsed = Math.Round(percentageUsed, 2),
                AlertSentAt = DateTime.UtcNow
            };

            await alertRepo.AddAsync(alert);
        }
    }
}



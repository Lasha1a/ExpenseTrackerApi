using ExpenseTracker.Application.Interfaces.Email;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExpenseTracker.Infrastructure.Background;

public class ReportJobWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ReportJobWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            var pendingJobs = await context.ReportJobs
                .Where(j => j.Status == "Pending")
                .ToListAsync(stoppingToken);

            foreach (var job in pendingJobs)
            {
                try
                {
                    await ProcessJob(job, context, emailService, stoppingToken);
                }
                catch (Exception ex)
                {
                    job.Status = "Failed";
                    job.UpdatedAt = DateTime.UtcNow;
                    Console.WriteLine($"Job {job.Id} failed: {ex.Message}");
                }
            }
            await context.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task ProcessJob(
        ReportJob job,
        DataContext context,
        IEmailService emailService,
        CancellationToken stoppingToken)
    {
        job.Status = "Processing";
        job.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync(stoppingToken);

        // get user email from DB
        var user = await context.Users.FindAsync(new object[] { job.UserId }, stoppingToken);
        if (user == null)
        {
            job.Status = "Failed";
            job.UpdatedAt = DateTime.UtcNow;
            return;
        }

        var emailBody =
    $"""
Report ID: {job.Id}
User ID: {job.UserId}
Type: {job.ReportType}
Created: {job.CreatedAt}
Generated: {DateTime.UtcNow}
""";

        await emailService.SendEmailAsync(
            user.Email,
            "Your Monthly Expense Report",
            emailBody
        );

        job.Status = "Completed";
        job.UpdatedAt = DateTime.UtcNow;
    }
}
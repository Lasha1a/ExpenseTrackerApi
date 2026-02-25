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
        // loop runs as long as the app is alive
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            // get pending jobs
            var pendingJobs = await context.ReportJobs
                .Where(j => j.Status == "Pending")
                .ToListAsync(stoppingToken);

            foreach (var job in pendingJobs)
            {
                try
                {
                    await ProcessJob(job, context, stoppingToken);
                }
                catch
                {
                    job.Status = "Failed";
                    job.UpdatedAt = DateTime.UtcNow;
                }
            }

            await context.SaveChangesAsync(stoppingToken);

            // wait 1 minute before next run
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task ProcessJob(
        ReportJob job,
        DataContext context,
        CancellationToken stoppingToken)
    {
        // mark as processing
        job.Status = "Processing";
        job.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync(stoppingToken);

        // create Reports folder (in API root)
        var reportsDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
        Directory.CreateDirectory(reportsDir);

        // create file
        var fileName = $"report_{job.Id}.txt";
        var filePath = Path.Combine(reportsDir, fileName);

        var content =
$"""
Report ID: {job.Id}
User ID: {job.UserId}
Type: {job.ReportType}
Created: {job.CreatedAt}
Generated: {DateTime.UtcNow}
""";

        await File.WriteAllTextAsync(filePath, content, stoppingToken);

        // mark as completed
        job.Status = "Completed";
        job.UpdatedAt = DateTime.UtcNow;
    }
}

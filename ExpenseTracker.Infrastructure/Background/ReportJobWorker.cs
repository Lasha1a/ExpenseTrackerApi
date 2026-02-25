using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Specifications.Reports;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Infrastructure.DataBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Background;

public class ReportJobWorker : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly DataContext _context;

    public ReportJobWorker(IServiceScopeFactory serviceScopeFactory, DataContext context)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessJobs();

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Delay between job processing cycles
        }
    }

    private async Task ProcessJobs()
    {
       using var scope = _serviceScopeFactory.CreateScope();

       var repo = scope.ServiceProvider.GetRequiredService<IRepository<ReportJob>>();

        var pendingJobs = await repo.ListAsync(new PendingReportJobsSpec());

        foreach (var job in pendingJobs)
        {
            await ProcessSingleJob(job, repo);
        }
    }

    private async Task ProcessSingleJob(ReportJob job, IRepository<ReportJob> repo)
    {
       //mark as processing
       job.Status = "Processing";
       job.UpdatedAt = DateTime.UtcNow;
       repo.Update(job);
       await _context.SaveChangesAsync();

        //create the report directory if it dont exists
        var reportsDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");

        Directory.CreateDirectory(reportsDir);

        // Create file for the report (simulate report generation)
        var fileName = $"report-{job.Id}.txt";
        var filePath = Path.Combine(reportsDir, fileName);

      var content =
      $@"Expense Tracker Report
      -----------------------
      Report Type: {job.ReportType}
      User Id: {job.UserId}
      Created At: {job.CreatedAt:yyyy-MM-dd HH:mm:ss}

      This is a generated report file.";

        await File.WriteAllTextAsync(filePath, content);

        // Mark as completed
        job.FileUrl = $"/reports/{fileName}";
        job.Status = "Completed";
        job.UpdatedAt = DateTime.UtcNow;

        repo.Update(job);
        await _context.SaveChangesAsync();

    }
}

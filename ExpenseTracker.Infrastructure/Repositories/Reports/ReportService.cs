using ExpenseTracker.Application.DTOs.Reports;
using ExpenseTracker.Application.Interfaces.Reports;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Repositories.Reports;

internal class ReportService : IReportService
{
    private readonly IRepository<ReportJob> _reportJobRepository;

    public ReportService(IRepository<ReportJob> reportJobRepository)
    {
        _reportJobRepository = reportJobRepository;
    }

    public async Task<Guid> CreateMonthlyReportAsync(CreateMonthlyReportRequest request)
    {
        var reportJob = new ReportJob
        {
            UserId = request.UserId,
            ReportType = "MonthlyExpenseSummary",
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };
        await _reportJobRepository.AddAsync(reportJob);
        return reportJob.Id;
    }


}

using ExpenseTracker.Application.DTOs.Reports;
using ExpenseTracker.Application.Interfaces.Reports;
using ExpenseTracker.Infrastructure.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    public readonly IReportService _reportService;
    public readonly DataContext _context;

    public ReportController(IReportService reportService, DataContext context)
    {
        _reportService = reportService;
        _context = context;
    }

    [HttpPost("monthly")]
    public async Task<IActionResult> CreateMonthlyReport([FromBody] CreateMonthlyReportRequest request)
    {
        var reportId = await _reportService.CreateMonthlyReportAsync(request);

        await _context.SaveChangesAsync();
        return Accepted(new
        {
            reportId,
            status = "pending"
        });
    }
}

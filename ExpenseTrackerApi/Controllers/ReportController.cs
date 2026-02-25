using ExpenseTracker.Application.DTOs.Reports;
using ExpenseTracker.Application.Interfaces.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    public readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpPost("monthly")]
    public async Task<IActionResult> CreateMonthlyReport([FromBody] CreateMonthlyReportRequest request)
    {
        var reportId = await _reportService.CreateMonthlyReportAsync(request);
        return Accepted(new
        {
            reportId,
            status = "pending"
        });
    }
}

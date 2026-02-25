using ExpenseTracker.Application.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Interfaces.Reports;

public interface IReportService
{
    Task<Guid> CreateMonthlyReportAsync(CreateMonthlyReportRequest request);
}

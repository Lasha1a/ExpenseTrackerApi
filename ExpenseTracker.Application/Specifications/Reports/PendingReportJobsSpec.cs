using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Specifications.Reports;

public class PendingReportJobsSpec : BaseSpecification<ReportJob>
{
    public PendingReportJobsSpec() : base(j => j.Status == "Pending")
    {
    }
}

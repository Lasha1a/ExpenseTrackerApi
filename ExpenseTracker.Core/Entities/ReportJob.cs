using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Entities;

public class ReportJob
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public string ReportType { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

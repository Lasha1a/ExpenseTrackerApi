using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Entities;

public class BudgetAlert
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int Month { get; set; }
    public decimal PercentageUsed { get; set; }
    public DateTime AlertSentAt { get; set; }
}

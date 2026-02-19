using ExpenseTracker.Core.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Entities;

public class Category : EntityBase
{

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string ColorHex { get; set; } = "#000000";
    public decimal MonthlyBudget { get; set; } = 0;
    public bool IsActive { get; set; } = false;

    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}

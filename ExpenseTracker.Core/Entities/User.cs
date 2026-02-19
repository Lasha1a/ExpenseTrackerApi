using ExpenseTracker.Core.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Entities;

public class User : EntityBase
{
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string CurrencyCode { get; set; } = "USD";
    public DateTime CreatedAt { get; set; }
    

    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}

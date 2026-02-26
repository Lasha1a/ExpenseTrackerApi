using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.ExpenseDtos;

public class ExpenseResponse //for get/list responses
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;

    public decimal Amount { get; set; }
    public string Description { get; set; } = null!;
    public DateOnly ExpenseDate { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

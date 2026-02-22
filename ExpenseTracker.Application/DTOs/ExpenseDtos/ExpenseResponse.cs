using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.ExpenseDtos;

internal class ExpenseResponse //for get/list responses
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = null!;

    public decimal Amount { get; init; }
    public string Description { get; init; } = null!;
    public DateTime ExpenseDate { get; init; }

    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

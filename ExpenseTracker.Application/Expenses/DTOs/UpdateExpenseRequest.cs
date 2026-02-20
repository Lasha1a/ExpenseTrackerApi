using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Expenses.DTOs;

internal class UpdateExpenseRequest
{
    public decimal Amount { get; init; }
    public string? Description { get; init; }
    public DateTime ExpenseDate { get; init; }

    public Guid CategoryId { get; init; }

}

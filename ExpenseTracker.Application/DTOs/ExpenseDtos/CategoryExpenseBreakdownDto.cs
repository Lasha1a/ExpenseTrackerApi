using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.ExpenseDtos;

public class CategoryExpenseBreakdownDto
{
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = null!;
    public decimal TotalAmount { get; init; }
}

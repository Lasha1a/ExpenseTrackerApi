using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.CategoriesDtos;

public class CategoryBudgetStatusDto
{
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = null!;

    public decimal MonthlyBudget { get; init; }
    public decimal SpentThisMonth { get; init; }

    public decimal? percentageUsed { get; init; }
    public bool IsExeeded { get; init; }
}

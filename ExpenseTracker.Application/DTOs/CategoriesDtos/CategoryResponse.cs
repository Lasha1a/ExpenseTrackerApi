using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.CategoriesDtos;

internal class CategoryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string ColorHex { get; init; } = "#FFFFFF";
    public decimal? MonthlyBudget { get; init; }
    public bool IsActive { get; init; }
}

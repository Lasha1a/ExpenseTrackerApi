using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.CategoriesDtos;

public class CreateCategoryRequest
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = null!;
    public string ColorHex { get; init; } = "#000000";
    public decimal MonthlyBudget { get; init; }

}

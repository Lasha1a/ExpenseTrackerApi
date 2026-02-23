using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Specifications.CategorySpecs;

public class ActiveCategoriesWithBudgetSpec : BaseSpecification<Category>
{
    public ActiveCategoriesWithBudgetSpec()
        : base(c => c.IsActive && c.MonthlyBudget > 0)
    {
    }
}

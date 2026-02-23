using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Specifications.BudgetSpecs;
public class BudgetAlertByCategoryMonthSpec : BaseSpecification<BudgetAlert>
{
    public BudgetAlertByCategoryMonthSpec(
        Guid userId,
        Guid categoryId,
        int year,
        int month)
        : base(a =>
            a.UserId == userId &&
            a.CategoryId == categoryId &&
            a.Year == year &&
            a.Month == month)
    {

    }
}

using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Specifications.BudgetSpecs;

public class BudgetAlertExistsSpec : BaseSpecification<BudgetAlert>
{
    // summary of the constructor: Initializes a new instance of the BudgetAlertExistsSpec class with the specified user ID, category ID, year, and month. It builds the criteria for checking if a budget alert exists based on these parameters.
    public BudgetAlertExistsSpec(
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

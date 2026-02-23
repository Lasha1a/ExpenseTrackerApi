using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Specifications;

public class ExpensesByCategoryAndMonthSpec : BaseSpecification<Expense>
{
    //summary of the constructor: Initializes a new instance of the ExpensesByCategoryAndMonthSpec class with the specified user ID, category ID, year, and month. It builds the criteria for filtering expenses based on these parameters.
    public ExpensesByCategoryAndMonthSpec(
        Guid userId,
        Guid categoryId,
        int year,
        int month)
        : base(BuildCriteria(userId, categoryId, year, month))
    {
    }
    // summary of the BuildCriteria method: Builds an expression that defines the criteria for filtering expenses based on the user ID, category ID, year, and month. It calculates the start and end dates for the specified month and returns a lambda expression that checks if an expense belongs to the specified user and category, and if its date falls within the calculated date range.
    private static Expression<Func<Expense, bool>> BuildCriteria(
        Guid userId,
        Guid categoryId,
        int year,
        int month)
    {
        var start = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = start.AddMonths(1);

        // The returned expression checks if an expense belongs to the specified user and category, and if its date falls within the calculated date range.
        return e =>
            e.UserId == userId &&
            e.CategoryId == categoryId &&
            e.ExpenseDate >= start &&
            e.ExpenseDate < end;
    }
}

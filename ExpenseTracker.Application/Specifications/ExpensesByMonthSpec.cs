using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Specifications;

internal class ExpensesByMonthSpec : BaseSpecification<Expense>
{
    // This specification is designed to retrieve expenses for a specific user, year, and month. It filters expenses based on the user ID, month, and year of the expense date. Additionally, it includes the related Category entity in the query results to provide more context about each expense.
    public ExpensesByMonthSpec(Guid userId, int year, int month) 
        : base(e =>
            e.UserId == userId &&
            e.ExpenseDate.Month == month &&
            e.ExpenseDate.Year == year
        )
    {
        AddInclude(e => e.Category); // Include the Category entity in the query results
    }
}

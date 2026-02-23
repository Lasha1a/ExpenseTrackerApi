using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Specifications.ExpenseSpecs;

internal class ExpensesByDateRangeSpec : BaseSpecification<Expense> //this specification is for expense entities
{
    public ExpensesByDateRangeSpec( //constructor takes userId, startDate and endDate as parameters
        Guid userId,
        DateTime startDate,
        DateTime endDate) : base(e =>  //base constructor takes a lambda expression that filters expenses based on userId and date range
         e.UserId == userId &&
         e.ExpenseDate >= startDate &&
         e.ExpenseDate <= endDate)
    {
        AddInclude(e => e.Category); //this line includes the related Category entity when querying expenses, allowing access to category details without additional queries
        ApplyOrderByDescending(e => e.ExpenseDate); //this line applies a descending order to the results based on the ExpenseDate, ensuring that the most recent expenses are returned first
    }
}

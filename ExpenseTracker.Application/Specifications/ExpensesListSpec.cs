using ExpenseTracker.Application.DTOs.ExpenseDtos;
using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Specifications;

public class ExpensesListSpec : BaseSpecification<Expense>
{
    public ExpensesListSpec(ExpenseListQuery query) // This constructor takes an ExpenseListQuery object as a parameter, which contains the filtering, pagination, and sorting parameters for the query. The base constructor is called with a lambda expression that defines the criteria for filtering expenses based on the user ID, category ID, and date range specified in the query.
        : base(e =>
            e.UserId == query.UserId &&
            (!query.CategoryId.HasValue || e.CategoryId == query.CategoryId) &&
            (!query.FromDate.HasValue || e.ExpenseDate >= query.FromDate) &&
            (!query.ToDate.HasValue || e.ExpenseDate <= query.ToDate)
        )
    {
       AddInclude(e => e.Category); // Include the Category entity in the query results

       ApplyOrderByDescending(e => e.ExpenseDate); // Default sorting by date in descending order

        ApplyPaging(
            (query.Page - 1) * query.PageSize,
            query.PageSize
        ); // Apply pagination based on the page number and page size
    }
}

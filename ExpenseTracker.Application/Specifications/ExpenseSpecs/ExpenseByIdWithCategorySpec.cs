using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Specifications.ExpenseSpecs;

public class ExpenseByIdWithCategorySpec : BaseSpecification<Expense>
{
    public ExpenseByIdWithCategorySpec(Guid id)
        : base(e => e.Id == id)
    {
        AddInclude(e => e.Category);
    }
}

using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Specifications.CategorySpecs;

public class CategoriesByUserSpec : BaseSpecification<Category>
{
    public CategoriesByUserSpec(Guid userId)
        : base(c => c.UserId == userId && c.IsActive)
    { }
}

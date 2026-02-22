using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Specifications;

// helper class for writing specifications.
public abstract class BaseSpecification<T> : ISpecification<T> 
{
    protected BaseSpecification() { } // This constructor allows us to create a specification without any criteria, which can be useful for listing all entities or when the criteria will be added later.

    protected BaseSpecification(Expression<Func<T, bool>> criteria) 
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>>? Criteria { get; } // This is the main criteria for filtering entities
    public List<Expression<Func<T, object>>> Includes { get; } = new(); // This is a list of related entities to include in the query results

    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    //paging
    public int? Skip { get; private set; }
    public int? Take { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected void AddInclude(Expression<Func<T, object>> include) // This method allows us to add related entities to include in the query results
    {
        Includes.Add(include);
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy) // This method allows us to specify the ordering of the query results
    {
        OrderBy = orderBy;
    }

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescending) // This method allows us to specify the ordering of the query results in descending order
    {
        OrderByDescending = orderByDescending;
    }

    // This method allows us to specify pagination parameters for the query results
    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

}

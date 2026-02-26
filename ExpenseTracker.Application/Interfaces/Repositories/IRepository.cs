using ExpenseTracker.Core.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Application.Specifications;

namespace ExpenseTracker.Application.Interfaces.Repositories;

public interface IRepository<T> where T : EntityBase
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<int> CountAsync(ISpecification<T> specification);
    Task<T?> FirstOrDefaultAsync(ISpecification<T> specification);
}

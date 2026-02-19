using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Specifications;
using ExpenseTracker.Core.common;
using ExpenseTracker.Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Repositories;

internal class GenericRepository<T> : IRepository<T> where T : EntityBase
{
    protected readonly DataContext _context;

    public GenericRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity) =>
       await _context.Set<T>().AddAsync(entity);


    public async Task<int> CountAsync(ISpecification<T> specification) =>
       await ApplySpecification(specification).CountAsync();

    public void Delete(T entity) =>
        _context.Set<T>().Remove(entity);


    public async Task<T?> GetByIdAsync(Guid id) =>
        await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IReadOnlyList<T>> ListAllAsync() =>
        await _context.Set<T>().ToListAsync();
    

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification) =>
        await ApplySpecification(specification).ToListAsync();


    public void Update(T entity) =>
       _context.Set<T>().Update(entity);


    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        IQueryable<T> query = _context.Set<T>();

        if(specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

        if(specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }

        if(specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        return query;
    }
}

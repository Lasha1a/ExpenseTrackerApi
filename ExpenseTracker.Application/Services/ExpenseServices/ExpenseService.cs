using ExpenseTracker.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Application.DTOs.ExpenseDtos;
using ExpenseTracker.Application.Specifications;

namespace ExpenseTracker.Application.Services.ExpenseServices;

public class ExpenseService
{
    private readonly IRepository<Expense> _repository;

    public ExpenseService(IRepository<Expense> repository)
    {
        _repository = repository;
    }


    //Create
    public async Task<Expense> CreateAsync(CreateExpenseRequest request)
    {
        if(request.Amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.");
        }

        var expense = new Expense
        {
            UserId = request.UserId,
            CategoryId = request.CategoryId,
            Amount = request.Amount,
            Description = request.Description,
            ExpenseDate = request.ExpenseDate,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(expense);

        return expense;
    }

    //read by id
    public async Task<Expense?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    //update
    public async Task UpdateAsync(Guid id, UpdateExpenseRequest request)
    {
        var expense = await _repository.GetByIdAsync(id);

        if (expense == null)
        {
            throw new KeyNotFoundException("Expense not found.");
        }

        if(request.Amount <= 0)
        {
              throw new ArgumentException("Amount must be greater than zero.");
        }

        expense.Amount = request.Amount;
        expense.Description = request.Description;
        expense.ExpenseDate = request.ExpenseDate;
        expense.CategoryId = request.CategoryId;
        expense.UpdatedAt = DateTime.UtcNow;

        _repository.Update(expense);
    }

    //delete
    public async Task DeleteAsync(Guid id)
    {
        var expense = await _repository.GetByIdAsync(id);
        if (expense == null)
        {
            throw new KeyNotFoundException("Expense not found.");
        }
        _repository.Delete(expense);
    }

    //list with filtering, pagination, and sorting
    public async Task<IReadOnlyList<Expense>> ListAsync(ExpenseListQuery query)
    {
        var spec = new ExpensesListSpec(query);
        return await _repository.ListAsync(spec);
    }
}

using ExpenseTracker.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Application.DTOs.ExpenseDtos;
using ExpenseTracker.Application.Specifications;
using ExpenseTracker.Application.DTOs.CSV;

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

    //import from csv
    public async Task<int> ImportFromCsvAsync(Guid userId, IEnumerable<CsvExpenseRow> rows, 
        IEnumerable<Category> categories)
    {
        var categoryLookup = categories // Create a lookup dictionary for category names to IDs, ignoring case
            .ToDictionary(c => c.Name.ToLower(), c => c.Id);

        var expenses = new List<Expense>();

        foreach (var row in rows) // Iterate through each CSV row
        {
            if(!categoryLookup.TryGetValue(row.CategoryName.ToLower(), out var categoryId))
            {
                continue; // Skip rows with unknown categories
            }

            expenses.Add(new Expense
            {
                UserId = userId,
                CategoryId = categoryId,
                Amount = row.Amount,
                Description = row.Description,
                ExpenseDate = row.ExpenseDate,
                CreatedAt = DateTime.UtcNow
            });
        }

        foreach (var expense in expenses)
        {
            await _repository.AddAsync(expense);
        }

        return expenses.Count;
    }

    public async Task<IReadOnlyList<CategoryExpenseBreakdownDto>> GetCategoryExpenseBreakdownAsync(Guid userId, int year, int month)
    {
        var spec = new ExpensesByMonthSpec(userId, year, month);
        var expenses = await _repository.ListAsync(spec);

        return expenses
            .GroupBy(e => new { e.CategoryId, e.Category.Name })
            .Select(g => new CategoryExpenseBreakdownDto
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.Name,
                TotalAmount = g.Sum(e => e.Amount)
            })
            .ToList();
    }
}

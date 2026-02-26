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
using ExpenseTracker.Application.Specifications.ExpenseSpecs;
using ExpenseTracker.Application.Interfaces.Caching;

namespace ExpenseTracker.Application.Services.ExpenseServices;

public class ExpenseService
{
    private readonly IRepository<Expense> _repository;
    private readonly ICacheService _cache;

    public ExpenseService(IRepository<Expense> repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }


    //Create
    public async Task<ExpenseResponse> CreateAsync(CreateExpenseRequest request)
    {

        var expense = new Expense
        {
            UserId = request.UserId,
            CategoryId = request.CategoryId,
            Amount = request.Amount,
            Description = request.Description,
            ExpenseDate = DateTime.SpecifyKind(request.ExpenseDate.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc),
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(expense);

        return new ExpenseResponse
        {
            Id = expense.Id,
            UserId = expense.UserId,
            CategoryId = expense.CategoryId,
            CategoryName = string.Empty, // Category name is not available at this point since we only have the CategoryId. It will be populated when retrieving the expense with GetByIdAsync, which includes the category details.
            Amount = expense.Amount,
            Description = expense.Description ?? string.Empty,
            ExpenseDate = request.ExpenseDate,
            CreatedAt = expense.CreatedAt,
            UpdatedAt = expense.UpdatedAt
        };

        
    }

    //read by id
    public async Task<ExpenseResponse?> GetByIdAsync(Guid id)
    {
        var cacheKey = $"expense:id:{id}"; // Generate a unique cache key for the expense based on its ID

        // Try to get the expense from the cache first. If it's found, return it immediately
        var cachedExpense = await _cache.GetAsync<ExpenseResponse>(cacheKey);
        if(cachedExpense != null)
        {
            return cachedExpense;
        }

        // If the expense is not found in the cache, retrieve it from the database
        var expense = await _repository.GetByIdAsync(id);
        if (expense == null)
        {
            throw new KeyNotFoundException("Expense not found.");
        }

        //map entity to response dto
        var expenseResponse = new ExpenseResponse
        {
            Id = expense.Id,
            UserId = expense.UserId,
            CategoryId = expense.CategoryId,
            CategoryName = expense.Category.Name,
            Amount = expense.Amount,
            Description = expense.Description,
            CreatedAt = expense.CreatedAt,
            UpdatedAt = expense.UpdatedAt
        };

        //Store the retrieved expense in the cache with an expiration time of 5 minutes. This allows subsequent requests for the same expense to be served from the cache, improving response times and reducing database load.
        await _cache.SetAsync(cacheKey, expenseResponse, TimeSpan.FromMinutes(5));

        return expenseResponse;
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

        // Invalidate the cache for this expense since it has been updated
        await _cache.RemoveAsync($"expense:id:{expense.Id}");
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

        // Invalidate the cache for this expense since it has been deleted
        await _cache.RemoveAsync($"expense:id:{expense.Id}"); 
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

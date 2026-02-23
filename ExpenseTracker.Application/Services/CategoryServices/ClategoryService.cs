using ExpenseTracker.Application.DTOs.CategoriesDtos;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Services.ExpenseServices;
using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Services.CategoryServices;
public class CategoryService
{
    private readonly IRepository<Category> _repository;
    private readonly  ExpenseService _expenseService;

    public CategoryService(IRepository<Category> repository, ExpenseService expenseService)
    {
        _repository = repository;
        _expenseService = expenseService;
    }

    //Create
    public async Task<Category> CreateAsync(CreateCategoryRequest request)
    {
        var category = new Category
        {
            UserId = request.UserId,
            Name = request.Name,
            ColorHex = request.ColorHex,
            MonthlyBudget = request.MonthlyBudget
        };

        await _repository.AddAsync(category);
        return category;
    }

    //read by id
    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    //update
    public async Task UpdateAsync(Guid id, UpdateCategoryRequest request)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null)
        {
            throw new KeyNotFoundException("Category not found.");
        }

        category.Name = request.Name;
        category.ColorHex = request.ColorHex;
        category.MonthlyBudget = request.MonthlyBudget;

        _repository.Update(category);
    }

    //soft delete
    public async Task DeleteAsync(Guid id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null)
        {
            throw new KeyNotFoundException("Category not found.");
        }
        category.IsActive = false;

        _repository.Update(category);
    }

    public async Task<IReadOnlyList<CategoryBudgetStatusDto>> GetMonthlyBudgetStatusAsync(Guid userId, int year, int month)
    {
        //get breakdown method from expense service that exists alrdy
        var breakdown = await _expenseService.GetCategoryExpenseBreakdownAsync(userId, year, month);

        var categories = await _repository.ListAsync(new Category);
    }
}

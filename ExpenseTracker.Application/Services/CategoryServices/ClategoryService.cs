using ExpenseTracker.Application.DTOs.CategoriesDtos;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Services.ExpenseServices;
using ExpenseTracker.Application.Specifications;
using ExpenseTracker.Application.Specifications.CategorySpecs;
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
            MonthlyBudget = request.MonthlyBudget,
            IsActive = true
        };

        await _repository.AddAsync(category);
        return category;
    }

    //read by id
    public async Task<Category?> GetByIdAsync(Guid id)
    {
        var category = await _repository.GetByIdAsync(id);

        return category is { IsActive: true } ? category : null;
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

    // get monthly budget status for each category for a user
    public async Task<IReadOnlyList<CategoryBudgetStatusDto>?> GetMonthlyBudgetStatusAsync(Guid userId, int year, int month)
    {
        var breakdown = await _expenseService.GetCategoryExpenseBreakdownAsync(userId, year, month);

        if(!breakdown.Any())
        {
            return null;
        }

        var categories = await _repository.ListAsync(new CategoriesByUserSpec(userId));
        var result = new List<CategoryBudgetStatusDto>();

        foreach(var category in categories)
        {
            var spent = breakdown.FirstOrDefault(b => b.CategoryId == category.Id)
                ?.TotalAmount ?? 0;

            decimal? percentagedUsed = null;
            bool isExceeded = false;

            //Only calculate percentage if there's a budget set (> 0) to avoid division by zero and meaningless percentages
            if (category.MonthlyBudget > 0)
            {
                percentagedUsed = (spent / category.MonthlyBudget) * 100;
                isExceeded = spent >= category.MonthlyBudget;
            }

            result.Add(new CategoryBudgetStatusDto
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
                MonthlyBudget = category.MonthlyBudget,
                SpentThisMonth = spent,
                percentageUsed = percentagedUsed,
                IsExeeded = isExceeded
            });
        }

        return result;
    }
}

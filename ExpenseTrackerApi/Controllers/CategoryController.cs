using ExpenseTracker.Application.DTOs.CategoriesDtos;
using ExpenseTracker.Application.Services.CategoryServices;
using ExpenseTracker.Infrastructure.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _categoryService;
    private readonly DataContext _context;

    public CategoryController(CategoryService categoryService, DataContext context)
    {
        _categoryService = categoryService;
        _context = context;
    }

    //create
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryRequest request)
    {
        var category = await _categoryService.CreateAsync(request);
        await _context.SaveChangesAsync();
        return Ok(category.Id);
    }

    //read by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        var response = new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            ColorHex = category.ColorHex,
            MonthlyBudget = category.MonthlyBudget,
            IsActive = category.IsActive
        };

        return Ok(response);
    }

    //update
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCategoryRequest request)
    {
        await _categoryService.UpdateAsync(id, request);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    //delete
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.DeleteAsync(id);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("budget-status")]
    public async Task<IActionResult> GetMonthlyBudgetStatus([FromQuery] Guid userId, [FromQuery] int year, [FromQuery] int month)
    {
        var result = await _categoryService.GetMonthlyBudgetStatusAsync(userId, month, year);
        return Ok(result);
    }
}

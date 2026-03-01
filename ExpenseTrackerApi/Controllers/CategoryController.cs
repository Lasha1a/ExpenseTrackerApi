using ExpenseTracker.Application.DTOs.CategoriesDtos;
using ExpenseTracker.Application.DTOs.Reports;
using ExpenseTracker.Application.Services.CategoryServices;
using ExpenseTracker.Infrastructure.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers;

[Authorize] // needs jwt token
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
            return NotFound("category not found or has been deleted");
        }

        return Ok(category);
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
    public async Task<IActionResult> GetMonthlyBudgetStatus([FromQuery] CreateMonthlyReportRequest request)
    {
        var result = await _categoryService.GetMonthlyBudgetStatusAsync(request.UserId, request.Year, request.Month);

        if (result == null || !result.Any())
            return NotFound("No budget data found for the specified period.");

        return Ok(result);
    }
}

using ExpenseTracker.Application.Expenses.DTOs;
using ExpenseTracker.Application.Services.ExpenseServices;
using ExpenseTracker.Infrastructure.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseController : ControllerBase
{
    private readonly ExpenseService _expenseService;
    private readonly DataContext _context;

    public ExpenseController(ExpenseService expenseService, DataContext context)
    {
        _expenseService = expenseService;
        _context = context;
    }

    //create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]CreateExpenseRequest request)
    {
       var expense = await _expenseService.CreateAsync(request);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);

    }

    //read by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var expense = await _expenseService.GetByIdAsync(id);
        if (expense == null)
        {
            return NotFound();
        }
        return Ok(expense);
    }

    //update
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExpenseRequest request)
    {
        await _expenseService.UpdateAsync(id, request);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    //delete
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _expenseService.DeleteAsync(id);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

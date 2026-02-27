using ExpenseTracker.Application.DTOs.CSV;
using ExpenseTracker.Application.DTOs.ExpenseDtos;
using ExpenseTracker.Application.Services.ExpenseServices;
using ExpenseTracker.Infrastructure.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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

    //list with pagination and filtering endpoint
    [HttpGet("List")]
    public async Task<IActionResult> List([FromQuery] ExpenseListQuery query)
    {
        var expenses = await _expenseService.ListAsync(query);

       if(!expenses.Any())
        {
            return NotFound("no expenses found");
        }
        return Ok(expenses);
    }


    //import from csv endpoint

    [HttpPost("import/csv")]
    public async Task<IActionResult> ImportCsv([FromQuery] Guid userId,IFormFile file)
    {
        if (file == null || file.Length == 0) // check if file is null or empty
            return BadRequest("File is empty");

        var rows = new List<CsvExpenseRow>(); // list to hold parsed CSV rows

        using var reader = new StreamReader(file.OpenReadStream()); // create a stream reader to read the uploaded file
        string? line; // variable to hold each line read from the CSV
        bool isHeader = true; // flag to skip the header row

        while ((line = reader.ReadLine()) != null) // read each line until the end of the file
        {
            if (isHeader)
            {
                isHeader = false;
                continue;
            }

            var parts = line.Split(','); // split the line into parts based on comma delimiter

            rows.Add(new CsvExpenseRow // create a new CsvExpenseRow object and add it to the list
            {
                Amount = decimal.Parse(parts[0], CultureInfo.InvariantCulture),
                Description = parts[1],
                ExpenseDate = DateTime.Parse(parts[2], CultureInfo.InvariantCulture),
                CategoryName = parts[3]
            });
        }

        // load categories for user
        var categories = _context.Categories
            .Where(c => c.UserId == userId && c.IsActive)
            .ToList();

        var importedCount = await _expenseService // call the service method to import expenses from CSV, passing the user ID, parsed rows, and categories
            .ImportFromCsvAsync(userId, rows, categories);

        await _context.SaveChangesAsync();

        return Ok(new // return the count of imported expenses in the response
        {
            imported = importedCount
        });
    }

    //expense breakdown by category for a given month and year
    [HttpGet("breakdown/category")]
    public async Task<IActionResult> GetExpenseBreakdownByCategory([FromQuery] Guid userId, [FromQuery] int year, [FromQuery] int month)
    {
        var result = await _expenseService.GetCategoryExpenseBreakdownAsync(userId, year, month);

        return Ok(result);
    }

}

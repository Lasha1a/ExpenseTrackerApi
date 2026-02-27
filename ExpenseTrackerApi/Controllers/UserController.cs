using ExpenseTracker.Application.DTOs.UserDtos;
using ExpenseTracker.Application.Services.UserServices;
using ExpenseTracker.Infrastructure.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly DataContext _context;

    public UserController(UserService userService, DataContext context)
    {
        _userService = userService;
        _context = context;
    }

    //create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]CreateUserRequest request)
    {
        var user = await _userService.CreateAsync(request);
        await _context.SaveChangesAsync();
        return Ok(new
        {
            user.Id,
            user.Email,
            user.FullName,
            user.CurrencyCode
        });
    }

    //read by id
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(new
        {
            user.Id,
            user.Email,
            user.FullName,
            user.CurrencyCode
        });
    }
    //login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginRequest request)
    {
        var user = await _userService.LoginAsync(request);
        if (user == null)
        {
            return Unauthorized("invalid email or password");
        }
        return Ok(new
        {
            user.Id,
            user.Email,
            user.FullName,
            user.CurrencyCode
        });

    }


}

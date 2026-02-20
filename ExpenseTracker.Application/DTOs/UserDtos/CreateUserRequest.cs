using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.UserDtos;

public class CreateUserRequest
{
    public string Email { get; init; } = null!;
    public string FullName { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string CurrencyCode { get; init; } = "USD";

}

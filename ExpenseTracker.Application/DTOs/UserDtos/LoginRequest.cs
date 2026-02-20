using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.UserDtos;

public class LoginRequest
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}

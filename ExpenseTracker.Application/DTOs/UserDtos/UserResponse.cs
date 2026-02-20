using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.UserDtos;

public class UserResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = null!;
    public string FullName { get; init; } = null!;
    public string CurrencyCode { get; init; } = "USD";
    public DateTime CreatedAt { get; init; }
}

using ExpenseTracker.Application.Interfaces.Security;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Repositories.Security;

public class AppPasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new(); // We can use the built-in PasswordHasher from ASP.NET Core Identity, which provides a secure way to hash passwords.

    public string Hash(string password) // Hash the password using the PasswordHasher
    {
        return _hasher.HashPassword(null!, password);
    }

    public bool Verify(string password, string passwordHash) // Verify the password against the stored hash
    {
        var result = _hasher.VerifyHashedPassword(null!, passwordHash, password);

        return result == PasswordVerificationResult.Success;
    }
}

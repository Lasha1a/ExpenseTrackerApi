using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Interfaces.JwtSettings;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string email, string fullName);
}

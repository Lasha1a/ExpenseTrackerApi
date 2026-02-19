using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Persistence.Options;

public class ConnectionStringOptions
{
    public string DefaultConnection { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.CSV;

internal class CsvExpenseRow
{
    public decimal Amount { get; init; }
    public string Description { get; init; } = null!;
    public DateTime ExpenseDate { get; init; }
    public string CategoryName { get; init; } = null!;


}

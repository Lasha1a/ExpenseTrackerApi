using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs.ExpenseDtos;

public class ExpenseListQuery //for list/getall requests with filtering, pagination, sorting used w specifications
{
    public Guid UserId { get; init; }
    public Guid? CategoryId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 3;
}

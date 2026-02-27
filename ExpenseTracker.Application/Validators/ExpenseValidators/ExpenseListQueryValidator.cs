using ExpenseTracker.Application.DTOs.ExpenseDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Validators.ExpenseValidators;

public class ExpenseListQueryValidator : AbstractValidator<ExpenseListQuery>
{
    public ExpenseListQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .NotEqual(Guid.Empty).WithMessage("UserId cannot be an empty GUID.");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(50).WithMessage("Page size cannot exceed 50.");
    }
}

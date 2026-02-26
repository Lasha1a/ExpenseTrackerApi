using ExpenseTracker.Application.DTOs.CategoriesDtos;
using ExpenseTracker.Application.DTOs.Reports;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Validators.ReportValidations;

public class BudgetStatusRequestValidator : AbstractValidator<CreateMonthlyReportRequest>
{
    public BudgetStatusRequestValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("UserId must be a valid GUID.");

            RuleFor(r => r.Month)
              .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");

            RuleFor(r => r.Year)
                 .GreaterThanOrEqualTo(2000).WithMessage("Year must be greater than or equal to 2000.")
                 .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Year cannot be in the future.");
    }
}

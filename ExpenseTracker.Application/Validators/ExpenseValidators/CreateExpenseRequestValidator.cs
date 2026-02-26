using ExpenseTracker.Application.DTOs.ExpenseDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Validators.ExpenseValidators;

public class CreateExpenseRequestValidator : AbstractValidator<CreateExpenseRequest>
{
    public CreateExpenseRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        RuleFor(x => x.ExpenseDate)
            .NotEmpty().WithMessage("ExpenseDate is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("ExpenseDate cannot be in the future.");
    }
        
}

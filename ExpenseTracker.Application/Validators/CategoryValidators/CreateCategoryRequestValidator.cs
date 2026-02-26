using ExpenseTracker.Application.DTOs.CategoriesDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Validators.CategoryValidators;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        RuleFor(x => x.ColorHex)
            .NotEmpty().WithMessage("ColorHex is required.")
            .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$").WithMessage("ColorHex must be a valid hex color code.");
        RuleFor(x => x.MonthlyBudget)
            .GreaterThanOrEqualTo(0).WithMessage("MonthlyBudget must be greater than or equal to 0.");
    }
}

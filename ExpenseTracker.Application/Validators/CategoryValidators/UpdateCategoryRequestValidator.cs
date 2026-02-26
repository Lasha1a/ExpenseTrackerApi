using ExpenseTracker.Application.DTOs.CategoriesDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Validators.CategoryValidators;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

        RuleFor(c => c.ColorHex)
            .NotEmpty().WithMessage("Color hex code is required.")
            .Matches("^#[0-9A-Fa-f]{6}$").WithMessage("ColorHex must be a valid hex color like #RRGGBB.");

        RuleFor(c => c.MonthlyBudget)
            .GreaterThanOrEqualTo(0).WithMessage("Monthly budget must be greater than or equal to zero.");
    }
}

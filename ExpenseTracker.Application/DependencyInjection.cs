using ExpenseTracker.Application.Validators.CategoryValidators;
using ExpenseTracker.Application.Validators.ReportValidations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services)
    {

        services.AddValidatorsFromAssemblyContaining<UpdateCategoryRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateCategoryRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<BudgetStatusRequestValidator>();

        return services;
    }
}

using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Specifications.Reports;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Infrastructure.DataBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Background;

public class ReportJobWorker : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ReportJobWorker(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

}
using ExpenseTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.DataBase;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BudgetAlert> BudgetAlerts { get; set; }
    public DbSet<ReportJob> ReportJobs { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This scans the WHOLE Infrastructure assembly,
        // including Persistence/Configurations
        modelBuilder.ApplyConfigurationsFromAssembly
            (typeof(DataContext).Assembly); 

        base.OnModelCreating(modelBuilder);
    }

}

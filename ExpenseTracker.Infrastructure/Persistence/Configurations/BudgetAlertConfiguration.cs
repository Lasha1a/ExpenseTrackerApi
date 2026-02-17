using ExpenseTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Persistence.Configurations;

// This class configures the BudgetAlert entity for Entity Framework Core
public class BudgetAlertConfiguration : IEntityTypeConfiguration<BudgetAlert>
{
    public void Configure(EntityTypeBuilder<BudgetAlert> builder)
    {
        builder.ToTable("BudgetAlerts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Month)
            .IsRequired();

        builder.Property(x => x.PercentageUsed)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(x => x.AlertSentAt)
            .HasDefaultValueSql("NOW()");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.UserId, x.CategoryId, x.Month }) // This creates a unique index on the combination of UserId, CategoryId, and Month, ensuring that a user cannot have multiple budget alerts for the same category and month.
            .IsUnique();
    }
}

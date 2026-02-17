using ExpenseTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.DataBase.Configurations;

// This class configures the Expense entity for Entity Framework Core
public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("Expenses"); // This specifies that the Expense entity should be mapped to a table named "Expenses" in the database.

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(10, 2)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.ExpenseDate)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(x => x.UpdatedAt);

        builder.HasOne(x => x.User) // This sets up a relationship where each Expense has one User
            .WithMany(u => u.Expenses)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Category) // This sets up a relationship where each Expense has one Category
            .WithMany(c => c.Expenses)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserId); // This creates an index on the UserId column to improve query performance when filtering expenses by user.
        builder.HasIndex(x => x.CategoryId);
    }
}

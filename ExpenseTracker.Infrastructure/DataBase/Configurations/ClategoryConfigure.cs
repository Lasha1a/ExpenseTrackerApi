using ExpenseTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.DataBase.Configurations;

// This class configures the Category entity for Entity Framework Core,
// specifying how it should be mapped to the database.
public class CategoryConfigure : IEntityTypeConfiguration<Category> 
{
    public void Configure(EntityTypeBuilder<Category> builder) // This method is called by the Entity Framework Core runtime to configure the Category entity.
    {
        builder.ToTable("Categories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ColorHex) //
            .HasMaxLength(7);

        builder.Property(x => x.MonthlyBudget)
            .HasPrecision(10,2);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(x => x.User) // This sets up a relationship where each Category has one User, and each User can have many Categories. The foreign key is UserId, and if a User is deleted, all their Categories will also be deleted (cascade delete).
            .WithMany(u => u.Categories)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new {x.UserId, x.Name }) // This creates a unique index on the combination of UserId and Name, ensuring that a user cannot have two categories with the same name.
            .IsUnique();
    }
}

using financial_planner.Models;
using Microsoft.EntityFrameworkCore;

namespace financial_planner.Database;

public class ApplicationDbContext: DbContext {
    public ApplicationDbContext(DbContextOptions options) : base(options) {
    }
    
    public DbSet<Expense>? Expenses { get; set; }
    public DbSet<Revenue>? Revenues { get; set; }
    public DbSet<Finance>? Finances { get; set; }

    // TODO: Add some preset values to all 3 tables. Add enum to Expense table Type property.
    // TODO: Connect to MS SQL Database. Add DB Migrations.
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Finance>(finance => {
            finance
                .Property(f => f.Month)
                .HasConversion(
                    m => m.Trim(), // Convert value before saving to the database
                    m => m.Trim() // Convert value when reading from the database
                    );
        });

        modelBuilder.Entity<Expense>(expense => {
            expense
                .Property(e => e.Amount)
                .HasConversion<double>();
        });
        
        modelBuilder.Entity<Revenue>(revenue => {
            revenue
                .Property(r => r.Amount)
                .HasConversion<double>();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}
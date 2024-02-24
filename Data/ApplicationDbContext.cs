using financial_planner.Models;
using Microsoft.EntityFrameworkCore;

namespace financial_planner.Data;

public class ApplicationDbContext: DbContext {
    public ApplicationDbContext(DbContextOptions options) : base(options) {
    }
    
    public DbSet<Expense>? Expenses { get; set; }
    public DbSet<Revenue>? Revenues { get; set; }
    public DbSet<Finance>? Finances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Finance>(finance => {
            finance
                .Property(f => f.Month)
                .HasConversion(
                    m => m.Trim(), // Convert value before saving to the database
                    m => m.Trim() // Convert value when reading from the database
                    );
            
            finance.HasData(
                new Finance() {FinanceId = 1, Year = 2023, Month = "December"},
                new Finance() {FinanceId = 2, Year = 2024, Month = "January"},
                new Finance() {FinanceId = 3, Year = 2024, Month = "February"}
            );
        });

        modelBuilder.Entity<Revenue>(revenue => {
            revenue
                .Property(r => r.Amount)
                .HasConversion<double>();
            
            revenue.HasData(
                new Revenue() { RevenueId = 1, Amount = 1500, Description = "Client A", FinanceId = 1 },
                new Revenue() { RevenueId = 2, Amount = 500, Description = "Client B", FinanceId = 1 },
                new Revenue() { RevenueId = 3, Amount = 2000, Description = "Client B", FinanceId = 2 },
                new Revenue() { RevenueId = 4, Amount = 2500, Description = "Client A", FinanceId = 3 }
            );
        });
        
        modelBuilder.Entity<Expense>(expense => {
            expense
                .Property(e => e.Amount)
                .HasConversion<double>();
            expense
                .Property(e => e.ExpenseTypeId)
                .HasConversion<int>();

            expense.HasData(
                new Expense() {
                    ExpenseId = 1, Amount = 300, ExpenseTypeId = ExpenseTypeId.Software, Description = "All the software", FinanceId = 1
                },
                new Expense() {
                    ExpenseId = 2, Amount = 700, ExpenseTypeId = ExpenseTypeId.Staff, Description = "New Staff", FinanceId = 2
                },
                new Expense() {
                    ExpenseId = 3, Amount = 500, ExpenseTypeId = ExpenseTypeId.Other, Description = "All the other", FinanceId = 3
                }
            );
        });

        modelBuilder.Entity<ExpenseType>(et => {
            et.Property(e => e.ExpenseTypeId)
                .HasConversion<int>();
            et.HasData(
                Enum.GetValues(typeof(ExpenseTypeId))
                    .Cast<ExpenseTypeId>()
                    .Select(eti => new ExpenseType() {
                        ExpenseTypeId = eti,
                        Type = eti.ToString()
                    })
            );
        });
        
        base.OnModelCreating(modelBuilder);
    }
}
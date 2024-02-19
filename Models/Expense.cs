using System.ComponentModel.DataAnnotations;

namespace financial_planner.Models;

public class Expense {
    public int ExpenseId { get; set; }
    
    [Required]
    public string Type { get; set; }  = null!;
    
    [Required]
    [MaxLength(50)]
    public string Description { get; set; }  = null!;
    
    [Required]
    public decimal Amount { get; set; }
    
    // these two define the foreign key relationship to the Finances table
    public int FinanceId { get; set; }
    public virtual Finance Finance { get; set; } = null!;
}
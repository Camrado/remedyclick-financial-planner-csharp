using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace financial_planner.Models;

public class Expense {
    public int ExpenseId { get; set; }
    
    [Required]
    public ExpenseTypeId ExpenseTypeId { get; set; }
    [JsonIgnore]
    public ExpenseType ExpenseType { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string Description { get; set; }  = null!;
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The Amount field must be greater than 0.")]
    public decimal Amount { get; set; }
    
    // these two define the foreign key relationship to the Finances table
    public int FinanceId { get; set; }
    [JsonIgnore]
    public virtual Finance Finance { get; set; } = null!;
}
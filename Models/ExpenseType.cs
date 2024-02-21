using System.ComponentModel.DataAnnotations;

namespace financial_planner.Models;

public class ExpenseType {
    public ExpenseTypeId ExpenseTypeId { get; set; }
    
    [Required]
    public string Type { get; set; }
    
    public List<Expense> Expenses { get; set; }
}
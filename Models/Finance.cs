using System.ComponentModel.DataAnnotations;

namespace financial_planner.Models;

public class Finance {
    public int FinanceId { get; set; }

    [Required] 
    public int Year { get; set; } = DateTime.Now.Year;
    
    [Required]
    [MaxLength(10)]
    public string Month { get; set; }  = null!;
    
    // define a navigation properties for related rows
    public virtual ICollection<Expense> Expenses { get; set; }
    public virtual ICollection<Revenue> Revenues { get; set; }

    public Finance() {
        // to enable developers add revenues and expenses to a Finance, we must initialize the navigation properties to empty collections
        Expenses = new HashSet<Expense>();
        Revenues = new HashSet<Revenue>();
    }
}
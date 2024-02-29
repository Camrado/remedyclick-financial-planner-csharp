using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace financial_planner.Models;

public class Revenue {
    public int RevenueId { get; set; }

    [Required] 
    [MaxLength(50)] 
    public string Description { get; set; } = null!;
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The Amount field must be greater than 0.")]
    public decimal Amount { get; set; }
    
    // these two define the foreign key relationship to the Finances table
    public int FinanceId { get; set; }
    [JsonIgnore]
    public virtual Finance Finance { get; set; } = null!;
}
using financial_planner.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace financial_planner.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
public class FinancesController: ControllerBase {
    private readonly ApplicationDbContext db;

    public FinancesController(ApplicationDbContext db) {
        this.db = db;
    }

    [HttpGet]
    public IActionResult GetFinances() {
        return Ok(db.Finances
            .Include(finance => finance.Expenses)
            .Include(finance => finance.Revenues)
            .ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetFinanceById(int id) {
        if (id < 0) {
            return BadRequest($"Invalid ID: {id}");
        }

        var finance = db.Finances
            .Include(finance => finance.Expenses)
            .Include(finance => finance.Revenues)
            .FirstOrDefault(finance => finance.FinanceId == id);

        if (finance is null) {
            return BadRequest($"No Finance with ID: {id}");
        }

        return Ok(finance);
    }
}
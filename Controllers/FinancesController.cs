using financial_planner.Data;
using financial_planner.Data.Extensions;
using Microsoft.AspNetCore.Mvc;
using financial_planner.Filters.ActionFilters;
using financial_planner.Models;

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
        return Ok(db.Finances.ProjectFinanceData());
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(Finance_ValidateFinanceIdFilterAttribute))]
    // TypeFilter instantiates TypeFilterAttribute - A filter that creates another filter of type ImplementationType,
    // retrieving missing constructor arguments from dependency injection if available there.
    public IActionResult GetFinanceById(int id) {
        var finance = HttpContext.Items["finance"] as IQueryable<Finance>;
        return Ok(finance.ProjectFinanceData().First());
    }

    [HttpPost]
    [TypeFilter(typeof(Finance_ValidateReqBodyFilterAttribute))]
    public IActionResult CreateFinance([FromBody] Finance finance) {
        db.Add(finance);
        db.SaveChanges();

        return CreatedAtAction(nameof(GetFinanceById),
            new { id = finance.FinanceId },
            finance);
    }

    [HttpPut("{id}")]
    [TypeFilter(typeof(Finance_ValidateFinanceIdFilterAttribute))]
    [TypeFilter(typeof(Finance_ValidateUpdateFinanceAttribute))]
    [TypeFilter(typeof(Finance_ValidateReqBodyFilterAttribute))]
    public IActionResult UpdateFinance(int id, [FromBody] Finance finance) {
        var financeToUpdate = (HttpContext.Items["finance"] as IQueryable<Finance>)?.First();

        financeToUpdate.Year = finance.Year;
        financeToUpdate.Month = finance.Month;

        db.SaveChanges();

        return Ok(financeToUpdate);
    }

    [HttpDelete("{id}")]
    [TypeFilter(typeof(Finance_ValidateFinanceIdFilterAttribute))]
    public IActionResult DeleteFinance(int id) {
        var financeToDelete = (HttpContext.Items["finance"] as IQueryable<Finance>)?.First();
        
        db.Finances.Remove(financeToDelete);
        db.SaveChanges();

        return Ok(financeToDelete);
    }
}
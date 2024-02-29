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
    private readonly ApplicationDbContext _db;

    public FinancesController(ApplicationDbContext db) {
        this._db = db;
    }

    [HttpGet]
    public IActionResult GetFinances() {
        return Ok(_db.Finances.ProjectFinanceData());
    }

    [HttpGet("{financeId}")]
    [TypeFilter(typeof(Finance_ValidateFinanceIdFilterAttribute))]
    // TypeFilter instantiates TypeFilterAttribute - A filter that creates another filter of type ImplementationType,
    // retrieving missing constructor arguments from dependency injection if available there.
    public IActionResult GetFinanceById(int financeId) {
        var finance = HttpContext.Items["finance"] as IQueryable<Finance>;
        return Ok(finance.ProjectFinanceData().First());
    }

    [HttpPost]
    [TypeFilter(typeof(Finance_ValidateReqBodyFilterAttribute))]
    public IActionResult CreateFinance([FromBody] Finance finance) {
        _db.Add(finance);
        _db.SaveChanges();

        return CreatedAtAction(nameof(GetFinanceById),
            new { financeId = finance.FinanceId },
            finance);
    }

    [HttpPut("{financeId}")]
    [TypeFilter(typeof(Finance_ValidateFinanceIdFilterAttribute))]
    [TypeFilter(typeof(Finance_ValidateUpdateFinanceAttribute))]
    [TypeFilter(typeof(Finance_ValidateReqBodyFilterAttribute))]
    public IActionResult UpdateFinance(int financeId, [FromBody] Finance finance) {
        var financeToUpdate = (HttpContext.Items["finance"] as IQueryable<Finance>)?.First();

        financeToUpdate.Year = finance.Year;
        financeToUpdate.Month = finance.Month;

        _db.SaveChanges();

        return Ok(financeToUpdate);
    }

    [HttpDelete("{financeId}")]
    [TypeFilter(typeof(Finance_ValidateFinanceIdFilterAttribute))]
    public IActionResult DeleteFinance(int financeId) {
        var financeToDelete = (HttpContext.Items["finance"] as IQueryable<Finance>)?.First();
        
        _db.Finances.Remove(financeToDelete);
        _db.SaveChanges();

        return Ok(financeToDelete);
    }
}
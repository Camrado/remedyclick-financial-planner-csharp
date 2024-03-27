using financial_planner.Caching;
using financial_planner.Data;
using financial_planner.Data.Extensions;
using Microsoft.AspNetCore.Mvc;
using financial_planner.Filters.ActionFilters;
using financial_planner.Models;
using Microsoft.AspNetCore.OutputCaching;

namespace financial_planner.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
public class FinancesController: ControllerBase {
    private readonly ApplicationDbContext _db;
    private readonly CacheHelper _cacheHelper;

    public FinancesController(ApplicationDbContext db, IOutputCacheStore cache) {
        _db = db;
        _cacheHelper = new CacheHelper(cache);
    }

    [HttpGet]
    [OutputCache(Tags = ["tag-finances"])]
    public IActionResult GetFinances() {
        return Ok(_db.Finances.ProjectFinanceData());
    }

    [HttpGet("{financeId}")]
    [OutputCache(PolicyName = "CacheFinanceById")]
    [TypeFilter(typeof(Finance_ValidateFinanceIdFilterAttribute))]
    // TypeFilter instantiates TypeFilterAttribute - A filter that creates another filter of type ImplementationType,
    // retrieving missing constructor arguments from dependency injection if available there.
    public IActionResult GetFinanceById(int financeId) {
        var finance = HttpContext.Items["finance"] as IQueryable<Finance>;
        return Ok(finance.ProjectFinanceData().First());
    }

    [HttpPost]
    [TypeFilter(typeof(Finance_ValidateReqBodyFilterAttribute))]
    public async Task<IActionResult> CreateFinance([FromBody] Finance finance, IOutputCacheStore cache) {
        _db.Add(finance); 
        await _db.SaveChangesAsync();

        await _cacheHelper.CleanFinanceCache();
        
        return CreatedAtAction(nameof(GetFinanceById),
            new { financeId = finance.FinanceId },
            finance);
    }

    [HttpPut("{financeId}")]
    [TypeFilter(typeof(Finance_ValidateFinanceIdFilterAttribute))]
    [TypeFilter(typeof(Finance_ValidateUpdateFinanceAttribute))]
    [TypeFilter(typeof(Finance_ValidateReqBodyFilterAttribute))]
    public async Task<IActionResult> UpdateFinance(int financeId, [FromBody] Finance finance, IOutputCacheStore cache) {
        var financeToUpdate = (HttpContext.Items["finance"] as IQueryable<Finance>)?.First();

        financeToUpdate.Year = finance.Year;
        financeToUpdate.Month = finance.Month;

        await _db.SaveChangesAsync();
        
        await _cacheHelper.CleanFinanceCache(financeId);

        return Ok(financeToUpdate);
    }

    [HttpDelete("{financeId}")]
    [TypeFilter(typeof(Finance_ValidateFinanceIdFilterAttribute))]
    public async Task<IActionResult> DeleteFinance(int financeId, IOutputCacheStore cache) {
        var financeToDelete = (HttpContext.Items["finance"] as IQueryable<Finance>)?.First();
        
        _db.Finances.Remove(financeToDelete);
        await _db.SaveChangesAsync();
        
        await _cacheHelper.CleanFinanceCache(financeId);

        return Ok(financeToDelete);
    }
}
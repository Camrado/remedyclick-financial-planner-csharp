using financial_planner.Caching;
using financial_planner.Data;
using financial_planner.Filters.ActionFilters;
using financial_planner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace financial_planner.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{v:apiVersion}/finances/{financeId}/[controller]")]
[TypeFilter(typeof(Finance_ValidateFinanceIdFilterAttribute))]
public class RevenuesController: ControllerBase {
	private readonly ApplicationDbContext _db;
	private readonly CacheHelper _cacheHelper;

	public RevenuesController(ApplicationDbContext db, IOutputCacheStore cache) {
		_db = db;
		_cacheHelper = new CacheHelper(cache);
	}

	[HttpGet]
	[OutputCache(Tags = ["tag-revenues"])]
	public IActionResult GetRevenues(int financeId) {
		return Ok(_db.Revenues.Where(r => r.FinanceId == financeId));
	}
	
	[HttpGet("{revenueId}")]
	[OutputCache(PolicyName = "CacheRevenueById")]
	[TypeFilter(typeof(Revenue_ValidateRevenueIdFilterAttribute))]
	public IActionResult GetRevenueById(int financeId, int revenueId) {
		var revenue = HttpContext.Items["revenue"] as IQueryable<Revenue>;
		return Ok(revenue.First());
	}

	[HttpPost]
	[Revenue_ValidateReqBodyFilter]
	public async Task<IActionResult> CreateRevenue(int financeId, [FromBody] Revenue revenue) {
		_db.Revenues.Add(revenue);
		await _db.SaveChangesAsync();

		await _cacheHelper.CleanRevenueCache(financeId);

		return CreatedAtAction(nameof(GetRevenueById),
		new { revenueId = revenue.RevenueId, financeId = revenue.FinanceId },
		revenue);
	}
	
	[HttpPut("{revenueId}")]
	[TypeFilter(typeof(Revenue_ValidateRevenueIdFilterAttribute))]
	[Revenue_ValidateReqBodyFilter]
	[Revenue_ValidateUpdateRevenue]
	public async Task<IActionResult> UpdateRevenue(int financeId, int revenueId, [FromBody] Revenue revenue) {
		var revenueToUpdate = (HttpContext.Items["revenue"] as IQueryable<Revenue>)?.First();

		revenueToUpdate.Amount = revenue.Amount;
		revenueToUpdate.Description = revenue.Description;

		await _db.SaveChangesAsync();

		await _cacheHelper.CleanRevenueCache(financeId, revenueId);
		
		return Ok(revenueToUpdate);
	}
	
	[HttpDelete("{revenueId}")]
	[TypeFilter(typeof(Revenue_ValidateRevenueIdFilterAttribute))]
	public async Task<IActionResult> DeleteRevenue(int financeId, int revenueId) {
		var revenueToDelete = (HttpContext.Items["revenue"] as IQueryable<Revenue>)?.First();

		_db.Revenues.Remove(revenueToDelete);
		await _db.SaveChangesAsync();
		
		await _cacheHelper.CleanRevenueCache(financeId, revenueId);

		return Ok(revenueToDelete);
	}
}
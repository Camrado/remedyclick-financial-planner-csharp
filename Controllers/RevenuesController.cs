using financial_planner.Data;
using financial_planner.Filters.ActionFilters;
using financial_planner.Models;
using Microsoft.AspNetCore.Mvc;

namespace financial_planner.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{v:apiVersion}/finances/{financeId}/[controller]")]
[TypeFilter(typeof(Finance_ValidateFinanceIdFilterAttribute))]
public class RevenuesController: ControllerBase {
	private readonly ApplicationDbContext _db;

	public RevenuesController(ApplicationDbContext db) {
		this._db = db;
	}

	[HttpGet]
	public IActionResult GetRevenues(int financeId) {
		return Ok(_db.Revenues.Where(r => r.FinanceId == financeId));
	}
	
	[HttpGet("{revenueId}")]
	[TypeFilter(typeof(Revenue_ValidateRevenueIdFilterAttribute))]
	public IActionResult GetRevenueById(int financeId, int revenueId) {
		var revenue = HttpContext.Items["revenue"] as IQueryable<Revenue>;
		return Ok(revenue.First());
	}

	[HttpPost]
	[Revenue_ValidateReqBodyFilter]
	public IActionResult CreateRevenue(int financeId, [FromBody] Revenue revenue) {
		_db.Revenues.Add(revenue);
		_db.SaveChanges();

		return Ok();
		
		// TODO: Fix this
		// return CreatedAtAction(nameof(GetRevenueById),
		// new { revenueId = revenue.RevenueId },
		// revenue);
	}
	
	// TODO: Check Put and Delete Requests

	[HttpPut("{revenueId}")]
	[TypeFilter(typeof(Revenue_ValidateRevenueIdFilterAttribute))]
	[TypeFilter(typeof(Revenue_ValidateUpdateRevenueAttribute))]
	public IActionResult UpdateRevenue(int financeId, int revenueId, [FromBody] Revenue revenue) {
		var revenueToUpdate = (HttpContext.Items["revenue"] as IQueryable<Revenue>)?.First();

		revenueToUpdate.Amount = revenue.Amount;
		revenueToUpdate.Description = revenue.Description;

		_db.SaveChanges();

		return Ok(revenueToUpdate);
	}
	
	[HttpDelete("{revenueId}")]
	[TypeFilter(typeof(Revenue_ValidateRevenueIdFilterAttribute))]
	public IActionResult DeleteRevenue(int financeId, int revenueId) {
		var revenueToDelete = (HttpContext.Items["revenue"] as IQueryable<Revenue>)?.First();

		_db.Revenues.Remove(revenueToDelete);
		_db.SaveChanges();

		return Ok(revenueToDelete);
	}
}
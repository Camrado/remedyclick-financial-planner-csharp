using System.ComponentModel.DataAnnotations;
using financial_planner.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace financial_planner.Filters.ActionFilters;

public class Revenue_ValidateRevenueIdFilterAttribute: ActionFilterAttribute {
	private readonly ApplicationDbContext _db;

	public Revenue_ValidateRevenueIdFilterAttribute(ApplicationDbContext db) {
		this._db = db;
	}

	public override void OnActionExecuting(ActionExecutingContext context) {
		base.OnActionExecuting(context);

		// context.ActionArguments has access to the arguments of an action method this attribute is attached to
		if (context.ActionArguments["revenueId"] is int revenueId) {
			if (revenueId <= 0) {
				context.ModelState.AddModelError("RevenueId", $"Revenue ID '{revenueId}' is invalid.");
				var problemDetails = new ValidationProblemDetails(context.ModelState);
				context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
			} else {
				var financeId = context.ActionArguments["financeId"] as int?;
				var revenue = _db.Revenues.Where(f => f.RevenueId == revenueId && f.FinanceId == financeId);

				if (!revenue.Any()) {
					context.ModelState.AddModelError("RevenueId", $"Revenue with ID '{revenueId}' for Finance with ID '{financeId}' does not exist.");
					var problemDetails = new ValidationProblemDetails(context.ModelState);
					context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult
				}

				context.HttpContext.Items["revenue"] = revenue;
			}
		}
	}
}
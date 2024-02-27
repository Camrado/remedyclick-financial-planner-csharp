using System.ComponentModel.DataAnnotations;
using financial_planner.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace financial_planner.Filters.ActionFilters;

public class Finance_ValidateFinanceIdFilterAttribute: ActionFilterAttribute {
	private readonly ApplicationDbContext _db;

	public Finance_ValidateFinanceIdFilterAttribute(ApplicationDbContext db) {
		this._db = db;
	}

	public override void OnActionExecuting(ActionExecutingContext context) {
		base.OnActionExecuting(context);

		// context.ActionArguments has access to the arguments of an action method this attribute is attached to
		if (context.ActionArguments["financeId"] is int financeId) {
			if (financeId <= 0) {
				context.ModelState.AddModelError("FinanceId", $"Finance ID '{financeId}' is invalid.");
				var problemDetails = new ValidationProblemDetails(context.ModelState);
				context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
			} else {
				var finance = _db.Finances.Where(f => f.FinanceId == financeId);

				if (!finance.Any()) {
					context.ModelState.AddModelError("FinanceId", $"Finance with ID '{financeId}' does not exist.");
					var problemDetails = new ValidationProblemDetails(context.ModelState);
					context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult
				}

				context.HttpContext.Items["finance"] = finance;
			}
		}
	}
}
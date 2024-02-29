using System.ComponentModel.DataAnnotations;
using financial_planner.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace financial_planner.Filters.ActionFilters;

public class Expense_ValidateExpenseIdFilterAttribute: ActionFilterAttribute {
	private readonly ApplicationDbContext _db;

	public Expense_ValidateExpenseIdFilterAttribute(ApplicationDbContext db) {
		this._db = db;
	}

	public override void OnActionExecuting(ActionExecutingContext context) {
		base.OnActionExecuting(context);

		// context.ActionArguments has access to the arguments of an action method this attribute is attached to
		if (context.ActionArguments["expenseId"] is int expenseId) {
			if (expenseId <= 0) {
				context.ModelState.AddModelError("ExpenseId", $"Expense ID '{expenseId}' is invalid.");
				var problemDetails = new ValidationProblemDetails(context.ModelState);
				context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
			} else {
				var financeId = context.ActionArguments["financeId"] as int?;
				var expense = _db.Expenses.Where(e => e.ExpenseId == expenseId && e.FinanceId == financeId);

				if (!expense.Any()) {
					context.ModelState.AddModelError("ExpenseId", $"Expense with ID '{expenseId}' for Finance with ID '{financeId}' does not exist.");
					var problemDetails = new ValidationProblemDetails(context.ModelState);
					context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult
				}

				context.HttpContext.Items["expense"] = expense;
			}
		}
	}
}
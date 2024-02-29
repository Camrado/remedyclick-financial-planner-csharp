using financial_planner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace financial_planner.Filters.ActionFilters;

public class Expense_ValidateUpdateExpenseAttribute: ActionFilterAttribute {
    public override void OnActionExecuting(ActionExecutingContext context) {
        base.OnActionExecuting(context);

        var expenseId = context.ActionArguments["expenseId"] as int?;
        var expense = context.ActionArguments["expense"] as Expense;

        if (expenseId.HasValue && expense is not null && expense.ExpenseId != expenseId) {
            context.ModelState.AddModelError("Expense", $"Expense IDs must be the same.");
            var problemDetails = new ValidationProblemDetails(context.ModelState);
            context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
        } 

    }
}
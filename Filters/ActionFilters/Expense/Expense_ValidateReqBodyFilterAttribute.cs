using financial_planner.Data;
using financial_planner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace financial_planner.Filters.ActionFilters;

public class Expense_ValidateReqBodyFilterAttribute: ActionFilterAttribute {
    private readonly ApplicationDbContext _db;

    public Expense_ValidateReqBodyFilterAttribute(ApplicationDbContext db) {
        this._db = db;
    }
    
    public override void OnActionExecuting(ActionExecutingContext context) {
        base.OnActionExecuting(context);
        
        // if it fails to cast the "Expense" type, its value will be assigned to null
        var expense = context.ActionArguments["expense"] as Expense;
        
        if (expense is null) {
            context.ModelState.AddModelError("Expense", $"Expense object is null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState);
            context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
        } else {
            var financeId = context.ActionArguments["financeId"] as int?;

            if (expense.FinanceId != financeId) {
                context.ModelState.AddModelError("Expense", $"Finance IDs must be the same.");
                var problemDetails = new ValidationProblemDetails(context.ModelState);
                context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
            }

            if (_db.ExpenseTypes.FirstOrDefault(et => et.ExpenseTypeId == expense.ExpenseTypeId) is null) {
                context.ModelState.AddModelError("Expense", $"Expense Type Id '{expense.ExpenseTypeId}' must be valid.");
                var problemDetails = new ValidationProblemDetails(context.ModelState);
                context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
            }
        }
    }
}
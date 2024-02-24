using financial_planner.Data;
using financial_planner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace financial_planner.Filters.ActionFilters;

public class Finance_ValidateUpdateFinanceAttribute: ActionFilterAttribute {
    private readonly ApplicationDbContext _db;

    public Finance_ValidateUpdateFinanceAttribute(ApplicationDbContext db) {
        this._db = db;
    }

    public override void OnActionExecuting(ActionExecutingContext context) {
        base.OnActionExecuting(context);

        var id = context.ActionArguments["id"] as int?;
        var finance = context.ActionArguments["finance"] as Finance;

        if (id.HasValue && finance is not null && finance.FinanceId != id) {
            context.ModelState.AddModelError("Finance", $"Finance IDs must be the same.");
            var problemDetails = new ValidationProblemDetails(context.ModelState);
            context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
        }
    }
}
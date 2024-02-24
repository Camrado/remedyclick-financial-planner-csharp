using financial_planner.Data;
using financial_planner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace financial_planner.Filters.ActionFilters;

public class Finance_ValidateReqBodyFilterAttribute: ActionFilterAttribute {
    private readonly ApplicationDbContext _db;

    public Finance_ValidateReqBodyFilterAttribute(ApplicationDbContext db) {
        this._db = db;
    }

    public override void OnActionExecuting(ActionExecutingContext context) {
        base.OnActionExecuting(context);

        // if it fails to cast the "Finance" type, its value will be assigned to null
        var finance = context.ActionArguments["finance"] as Finance;

        if (finance is null) {
            context.ModelState.AddModelError("Finance", $"Finance object is null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState);
            context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
        } else {
            var existingFinance = _db.Finances.FirstOrDefault(f =>
                f.Month.ToLower() == finance.Month.ToLower() &&
                f.Year == finance.Year);

            if (existingFinance is not null) {
                context.ModelState.AddModelError("Finance", $"Finance already exists.");
                var problemDetails = new ValidationProblemDetails(context.ModelState);
                context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
            }
        }
    }
}
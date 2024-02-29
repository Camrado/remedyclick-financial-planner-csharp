using financial_planner.Data;
using financial_planner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace financial_planner.Filters.ActionFilters;

public class Revenue_ValidateUpdateRevenueAttribute: ActionFilterAttribute {
    public override void OnActionExecuting(ActionExecutingContext context) {
        base.OnActionExecuting(context);

        var revenueId = context.ActionArguments["revenueId"] as int?;
        var revenue = context.ActionArguments["revenue"] as Revenue;

        if (revenueId.HasValue && revenue is not null && revenue.RevenueId != revenueId) {
            context.ModelState.AddModelError("Revenue", $"Revenue IDs must be the same.");
            var problemDetails = new ValidationProblemDetails(context.ModelState);
            context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
        } 
    }
}
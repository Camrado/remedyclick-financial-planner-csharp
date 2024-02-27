using financial_planner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace financial_planner.Filters.ActionFilters;

public class Revenue_ValidateReqBodyFilterAttribute: ActionFilterAttribute {
    public override void OnActionExecuting(ActionExecutingContext context) {
        base.OnActionExecuting(context);

        // if it fails to cast the "Revenue" type, its value will be assigned to null
        var revenue = context.ActionArguments["revenue"] as Revenue;
        
        if (revenue is null) {
            context.ModelState.AddModelError("Revenue", $"Revenue object is null.");
            var problemDetails = new ValidationProblemDetails(context.ModelState);
            context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
        }
        else {
            var financeId = context.ActionArguments["financeId"] as int?;

            if (revenue.FinanceId != financeId) {
                context.ModelState.AddModelError("Revenue", $"Finance IDs must be the same.");
                var problemDetails = new ValidationProblemDetails(context.ModelState);
                context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
            }
        }
    }
}
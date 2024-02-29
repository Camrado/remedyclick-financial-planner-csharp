﻿using financial_planner.Data;
using financial_planner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace financial_planner.Filters.ActionFilters;

public class Revenue_ValidateUpdateRevenueAttribute: ActionFilterAttribute {
    private readonly ApplicationDbContext _db;

    public Revenue_ValidateUpdateRevenueAttribute(ApplicationDbContext db) {
        this._db = db;
    }

    public override void OnActionExecuting(ActionExecutingContext context) {
        base.OnActionExecuting(context);

        var revenueId = context.ActionArguments["revenueId"] as int?;
        var revenue = context.ActionArguments["revenue"] as Revenue;
        var financeId = context.ActionArguments["financeId"] as int?;

        if (revenueId.HasValue && revenue is not null && revenue.RevenueId != revenueId) {
            context.ModelState.AddModelError("Revenue", $"Revenue IDs must be the same.");
            var problemDetails = new ValidationProblemDetails(context.ModelState);
            context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
        } else if (financeId.HasValue && revenue is not null && revenue.FinanceId != financeId) {
            context.ModelState.AddModelError("Revenue", $"Finance IDs must be the same.");
            var problemDetails = new ValidationProblemDetails(context.ModelState);
            context.Result = new BadRequestObjectResult(problemDetails); // executes IActionResult 
        }

    }
}
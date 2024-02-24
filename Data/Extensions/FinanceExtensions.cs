using financial_planner.Models;
using Microsoft.EntityFrameworkCore;

namespace financial_planner.Data.Extensions;

public static class FinanceExtensions {
    public static IQueryable<object> ProjectFinanceData(this IQueryable<Finance> query) {
        return query
            .Include(finance => finance.Expenses).ThenInclude(e => e.ExpenseType)
            .Include(finance => finance.Revenues)
            .Select(f => new {
                f.FinanceId,
                f.Year,
                f.Month,
                Revenues = f.Revenues.Select(r => new {
                    r.RevenueId,
                    r.Amount,
                    r.Description
                }),
                Expenses = f.Expenses.Select(e => new {
                    e.ExpenseId,
                    e.Amount,
                    e.ExpenseType.Type,
                    e.Description
                })
            });
    }
}
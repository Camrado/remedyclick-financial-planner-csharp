using financial_planner.Models;
using Microsoft.EntityFrameworkCore;

namespace financial_planner.Data.Extensions;

public static class ExpenseExtensions {
	public static IQueryable<object> ProjectExpenseData(this IQueryable<Expense> query) {
		return query
			.Include(e => e.ExpenseType)
			.Select(e => new {
				e.ExpenseId,
				e.Amount,
				e.ExpenseType.Type,
				e.Description
			});
	}
}
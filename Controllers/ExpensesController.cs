using financial_planner.Data;
using financial_planner.Data.Extensions;
using financial_planner.Filters.ActionFilters;
using financial_planner.Models;
using Microsoft.AspNetCore.Mvc;

namespace financial_planner.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{v:apiVersion}/finances/{financeId}/[controller]")]
[TypeFilter(typeof(Revenue_ValidateRevenueIdFilterAttribute))]
public class ExpensesController: ControllerBase {
    private readonly ApplicationDbContext _db;

    public ExpensesController(ApplicationDbContext db) {
        this._db = db;
    }

    // TODO: Check all these routes 
    
    [HttpGet]
    public IActionResult GetExpenses(int financeId) {
        return Ok(_db.Expenses.Where(e => e.FinanceId == financeId).ProjectExpenseData());
    }

    [HttpGet("{expenseId}")]
    [TypeFilter(typeof(Expense_ValidateExpenseIdFilterAttribute))]
    public IActionResult GetExpenseById(int financeId, int expenseId) {
        var expense = HttpContext.Items["expense"] as IQueryable<Expense>;
        return Ok(expense.ProjectExpenseData().First());
    }

    [HttpDelete("{expenseId}")]
    [TypeFilter(typeof(Expense_ValidateExpenseIdFilterAttribute))]
    public IActionResult DeleteExpense(int financeId, int expenseId) {
        var expenseToDelete = (HttpContext.Items["expense"] as IQueryable<Expense>)?.First();

        _db.Expenses.Remove(expenseToDelete);
        _db.SaveChanges();

        return Ok(expenseToDelete);
    }
}
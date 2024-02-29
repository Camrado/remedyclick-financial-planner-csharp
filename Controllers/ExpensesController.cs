using financial_planner.Data;
using financial_planner.Data.Extensions;
using financial_planner.Filters.ActionFilters;
using financial_planner.Models;
using Microsoft.AspNetCore.Mvc;

namespace financial_planner.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{v:apiVersion}/finances/{financeId}/[controller]")]
[TypeFilter(typeof(Finance_ValidateFinanceIdFilterAttribute))]
public class ExpensesController: ControllerBase {
    private readonly ApplicationDbContext _db;

    public ExpensesController(ApplicationDbContext db) {
        this._db = db;
    }

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

    [HttpPost]
    [TypeFilter(typeof(Expense_ValidateReqBodyFilterAttribute))]
    public IActionResult CreateExpense(int financeId, [FromBody] Expense expense) {
        _db.Expenses.Add(expense);
        _db.SaveChanges();

        return CreatedAtAction(nameof(GetExpenseById),
            new { expenseId = expense.ExpenseId, financeId = expense.FinanceId },
            expense);
    }

    [HttpPut("{expenseId}")]
    [TypeFilter(typeof(Expense_ValidateExpenseIdFilterAttribute))]
    [TypeFilter(typeof(Expense_ValidateReqBodyFilterAttribute))]
    [Expense_ValidateUpdateExpense]
    public IActionResult UpdateExpense(int financeId, int expenseId, [FromBody] Expense expense) {
        var expenseToUpdate = (HttpContext.Items["expense"] as IQueryable<Expense>)?.First();

        expenseToUpdate.Amount = expense.Amount;
        expenseToUpdate.Description = expense.Description;
        expenseToUpdate.ExpenseTypeId = expense.ExpenseTypeId;

        _db.SaveChanges();

        return Ok(expenseToUpdate);
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
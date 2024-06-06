using financial_planner.Caching;
using financial_planner.Data;
using financial_planner.Data.Extensions;
using financial_planner.Filters.ActionFilters;
using financial_planner.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace financial_planner.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{v:apiVersion}/finances/{financeId}/[controller]")]
[TypeFilter(typeof(Finance_ValidateFinanceIdFilterAttribute))]
public class ExpensesController: ControllerBase {
    private readonly ApplicationDbContext _db;
    private readonly CacheHelper _cacheHelper;

    public ExpensesController(ApplicationDbContext db, IOutputCacheStore cache) {
        _db = db;
        _cacheHelper = new CacheHelper(cache);
    }

    [HttpGet]
    [OutputCache(Tags = ["tag-expenses"])]
    public IActionResult GetExpenses(int financeId) {
        return Ok(_db.Expenses.Where(e => e.FinanceId == financeId).ProjectExpenseData());
    }

    [HttpGet("{expenseId}")]
    [OutputCache(PolicyName = "CacheExpenseById")]
    [TypeFilter(typeof(Expense_ValidateExpenseIdFilterAttribute))]
    public IActionResult GetExpenseById(int financeId, int expenseId) {
        var expense = HttpContext.Items["expense"] as IQueryable<Expense>;
        return Ok(expense.ProjectExpenseData().First());
    }

    [HttpPost]
    [TypeFilter(typeof(Expense_ValidateReqBodyFilterAttribute))]
    public async Task<IActionResult> CreateExpense(int financeId, [FromBody] Expense expense, IOutputCacheStore cache) {
        _db.Expenses.Add(expense);
        await _db.SaveChangesAsync();

        await _cacheHelper.CleanExpenseCacheAsync(financeId);

        return CreatedAtAction(nameof(GetExpenseById),
            new { expenseId = expense.ExpenseId, financeId = expense.FinanceId },
            expense);
    }

    [HttpPut("{expenseId}")]
    [TypeFilter(typeof(Expense_ValidateExpenseIdFilterAttribute))]
    [TypeFilter(typeof(Expense_ValidateReqBodyFilterAttribute))]
    [Expense_ValidateUpdateExpense]
    public async Task<IActionResult> UpdateExpense(int financeId, int expenseId, [FromBody] Expense expense, IOutputCacheStore cache) {
        var expenseToUpdate = (HttpContext.Items["expense"] as IQueryable<Expense>)?.First();

        expenseToUpdate.Amount = expense.Amount;
        expenseToUpdate.Description = expense.Description;
        expenseToUpdate.ExpenseTypeId = expense.ExpenseTypeId;

        await _db.SaveChangesAsync();
        
        await _cacheHelper.CleanExpenseCacheAsync(financeId, expenseId);

        return Ok(expenseToUpdate);
    }

    [HttpDelete("{expenseId}")]
    [TypeFilter(typeof(Expense_ValidateExpenseIdFilterAttribute))]
    public async Task<IActionResult> DeleteExpense(int financeId, int expenseId, IOutputCacheStore cache) {
        var expenseToDelete = (HttpContext.Items["expense"] as IQueryable<Expense>)?.First();

        _db.Expenses.Remove(expenseToDelete);
        await _db.SaveChangesAsync();
        
        await _cacheHelper.CleanExpenseCacheAsync(financeId, expenseId);

        return Ok(expenseToDelete);
    }
}
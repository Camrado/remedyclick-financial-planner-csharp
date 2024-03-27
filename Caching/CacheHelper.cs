using Microsoft.AspNetCore.OutputCaching;

namespace financial_planner.Caching;

public class CacheHelper {
    private readonly IOutputCacheStore _cache;

    public CacheHelper(IOutputCacheStore cache) {
        _cache = cache;
    }
    
    public async Task CleanFinanceCache() {
        // Cleaning cache for finances
        await _cache.EvictByTagAsync("tag-finances", default);
    }
    
    public async Task CleanFinanceCache(int financeId) {
        // Cleaning cache for finances
        await _cache.EvictByTagAsync("tag-finances", default);
        // Cleaning cache for tag -> 'finance-FINANCE_ID' 
        await _cache.EvictByTagAsync($"finance-{financeId}", default);
    }

    public async Task CleanRevenueCache(int financeId) {
        // Cleaning cache for revenues
        await _cache.EvictByTagAsync("tag-revenues", default);
        
        // Cleaning cache for finances
        await _cache.EvictByTagAsync("tag-finances", default);
        // Cleaning cache for tag -> 'finance-FINANCE_ID'
        await _cache.EvictByTagAsync($"finance-{financeId}", default);
    }
    
    public async Task CleanRevenueCache(int financeId, int revenueId) {
        // Cleaning cache for revenues
        await _cache.EvictByTagAsync("tag-revenues", default);
        // Cleaning cache for tag -> 'revenue-REVENUE_ID'
        await _cache.EvictByTagAsync($"revenue-{revenueId}", default);
        
        // Cleaning cache for finances
        await _cache.EvictByTagAsync("tag-finances", default);
        // Cleaning cache for tag -> 'finance-FINANCE_ID'
        await _cache.EvictByTagAsync($"finance-{financeId}", default);
    }
    
    public async Task CleanExpenseCache(int financeId) {
        // Cleaning cache for expenses
        await _cache.EvictByTagAsync("tag-expenses", default);
        
        // Cleaning cache for finances
        await _cache.EvictByTagAsync("tag-finances", default);
        // Cleaning cache for tag -> 'finance-FINANCE_ID'
        await _cache.EvictByTagAsync($"finance-{financeId}", default);
    }
    
    public async Task CleanExpenseCache(int financeId, int expenseId) {
        // Cleaning cache for expenses
        await _cache.EvictByTagAsync("tag-expenses", default);
        // Cleaning cache for tag -> 'expense-EXPENSE_ID'
        await _cache.EvictByTagAsync($"expense-{expenseId}", default);
        
        // Cleaning cache for finances
        await _cache.EvictByTagAsync("tag-finances", default);
        // Cleaning cache for tag -> 'finance-FINANCE_ID'
        await _cache.EvictByTagAsync($"finance-{financeId}", default);
    }
}
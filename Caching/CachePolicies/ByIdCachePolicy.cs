using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace financial_planner.Caching.CachePolicies;

public class ByIdCachePolicy: IOutputCachePolicy {
    private readonly string _idName;
    
    public ByIdCachePolicy(string idName) {
        _idName = idName;
    }
    
    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation) {
        var idRouteVal = context.HttpContext.Request.RouteValues[_idName];

        if (idRouteVal is null) {
            return ValueTask.CompletedTask;
        }

        // "finance-21", "revenue-2", etc.
        context.Tags.Add($"{_idName[..^2]}-{idRouteVal}");

        var attemptOutputCaching = AttemptOutputCaching(context);
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = attemptOutputCaching;
        context.AllowCacheStorage = attemptOutputCaching;
        context.AllowLocking = true;
        
        // Vary by any query by default
        context.CacheVaryByRules.QueryKeys = "*";
        
        return ValueTask.CompletedTask;
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation) {
        return ValueTask.CompletedTask;
    }

    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation) {
        var response = context.HttpContext.Response;

        // Verify existence of cookie headers
        if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie)) {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }

        // Check response code
        if (response.StatusCode != StatusCodes.Status200OK && 
            response.StatusCode != StatusCodes.Status301MovedPermanently) {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }

        return ValueTask.CompletedTask;
    }
    
    private static bool AttemptOutputCaching(OutputCacheContext context)
    {
        // Check if the current request fulfills the requirements to be cached
        var request = context.HttpContext.Request;

        // Verify the method
        if (!HttpMethods.IsGet(request.Method) && !HttpMethods.IsHead(request.Method)) {
            return false;
        }

        // Verify existence of authorization headers
        if (!StringValues.IsNullOrEmpty(request.Headers.Authorization) || 
            request.HttpContext.User.Identity?.IsAuthenticated == true) {
            return false;
        }

        return true;
    }
}
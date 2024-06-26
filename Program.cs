using System.Text.Json.Serialization;
using financial_planner.Caching.CachePolicies;
using financial_planner.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// MS SQL Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("FinancialPlannerStoreManagement"));
});

// Controllers
builder.Services
    .AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Financial Planner API v1", Version = "version 1"});
});

// API Versioning
builder.Services.AddVersionedApiExplorer(options => {
    options.GroupNameFormat = "'v'VVV";
});
builder.Services.AddApiVersioning(options => {
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
});

// Output Caching
builder.Services.AddOutputCache(options => {
    options.AddBasePolicy(cacheBuilder => {
        cacheBuilder.Expire(TimeSpan.FromMinutes(10));
    });
    options.AddPolicy("CacheFinanceById", new ByIdCachePolicy("financeId"));
    options.AddPolicy("CacheExpenseById", new ByIdCachePolicy("expenseId"));
    options.AddPolicy("CacheRevenueById", new ByIdCachePolicy("revenueId"));
});

// Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Financial Planner API v1");
    });
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseOutputCache();

app.MapControllers();

app.Run();

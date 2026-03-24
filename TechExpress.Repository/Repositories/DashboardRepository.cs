using Microsoft.EntityFrameworkCore;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Enums;

namespace TechExpress.Repository.Repositories;

public sealed record MonthlyRevenueByMonthData(
    int Year,
    int Month,
    decimal Revenue);

public sealed record ProductRevenueData(
    Guid ProductId,
    string ProductName,
    int TotalQuantitySold,
    decimal Revenue);

public class DashboardRepository
{
    private readonly ApplicationDbContext _context;

    public DashboardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MonthlyRevenueByMonthData>> GetMonthlyRevenueByMonthAsync(
        Guid? brandId,
        Guid? categoryId,
        DateTimeOffset startMonth,
        DateTimeOffset endMonth,
        CancellationToken ct)
    {
        var paidOrderIds = _context.Payments
            .AsNoTracking()
            .Where(p => p.Status == PaymentStatus.Success)
            .Select(p => p.OrderId)
            .Distinct();

        return await (
            from oi in _context.OrderItems.AsNoTracking()
            join o in _context.Orders.AsNoTracking() on oi.OrderId equals o.Id
            join p in _context.Products.AsNoTracking() on oi.ProductId equals p.Id
            where paidOrderIds.Contains(o.Id)
            where o.Status == OrderStatus.Completed
            where oi.UnitPrice > 0
            where !oi.IsFreeItem
            where p.Status == ProductStatus.Available
            where o.OrderDate >= startMonth
            where o.OrderDate < endMonth.AddMonths(1)
            where (!brandId.HasValue || p.BrandId == brandId.Value)
            where (!categoryId.HasValue || p.CategoryId == categoryId.Value)
            group new { oi, o } by new { o.OrderDate.Year, o.OrderDate.Month } into g
            select new MonthlyRevenueByMonthData(
                g.Key.Year,
                g.Key.Month,
                g.Sum(x => x.oi.Quantity * x.oi.UnitPrice))
        ).ToListAsync(ct);
    }

    public async Task<List<ProductRevenueData>> GetProductRevenueStatsAsync(
        Guid? brandId,
        Guid? categoryId,
        DateTimeOffset startMonth,
        DateTimeOffset endMonth,
        CancellationToken ct)
    {
        var paidOrderIds = _context.Payments
            .AsNoTracking()
            .Where(p => p.Status == PaymentStatus.Success)
            .Select(p => p.OrderId)
            .Distinct();

        return await (
            from oi in _context.OrderItems.AsNoTracking()
            join o in _context.Orders.AsNoTracking() on oi.OrderId equals o.Id
            join p in _context.Products.AsNoTracking() on oi.ProductId equals p.Id
            where paidOrderIds.Contains(o.Id)
            where o.Status == OrderStatus.Completed
            where oi.UnitPrice > 0
            where !oi.IsFreeItem
            where p.Status == ProductStatus.Available
            where o.OrderDate >= startMonth
            where o.OrderDate < endMonth.AddMonths(1)
            where (!brandId.HasValue || p.BrandId == brandId.Value)
            where (!categoryId.HasValue || p.CategoryId == categoryId.Value)
            group new { oi, p } by new { p.Id, p.Name } into g
            select new ProductRevenueData(
                g.Key.Id,
                g.Key.Name,
                g.Sum(x => x.oi.Quantity),
                g.Sum(x => x.oi.Quantity * x.oi.UnitPrice))
        ).ToListAsync(ct);
    }

    public async Task<string?> GetBrandNameAsync(Guid brandId, CancellationToken ct)
    {
        return await _context.Brands.AsNoTracking()
            .Where(b => b.Id == brandId)
            .Select(b => b.Name)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<string?> GetCategoryNameAsync(Guid categoryId, CancellationToken ct)
    {
        return await _context.Categories.AsNoTracking()
            .Where(c => c.Id == categoryId)
            .Select(c => c.Name)
            .FirstOrDefaultAsync(ct);
    }
}

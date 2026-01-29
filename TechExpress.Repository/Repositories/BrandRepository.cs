using Microsoft.EntityFrameworkCore;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Models;

namespace TechExpress.Repository.Repositories;

public class BrandRepository
{
    private readonly ApplicationDbContext _context;

    public BrandRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Brand?> FindByIdAsync(Guid id)
    {
        return await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Brand?> FindByIdWithTrackingAsync(Guid id)
    {
        return await _context.Brands.AsTracking().FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<(List<Brand> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchName,
        DateTimeOffset? createdFrom,
        DateTimeOffset? createdTo)
    {
        var query = _context.Brands
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchName))
        {
            var keyword = searchName.Trim();
            query = query.Where(b => EF.Functions.Like(b.Name, $"%{keyword}%"));
        }

        if (createdFrom.HasValue)
        {
            query = query.Where(b => b.CreatedAt >= createdFrom.Value);
        }

        if (createdTo.HasValue)
        {
            query = query.Where(b => b.CreatedAt <= createdTo.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(b => b.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(Brand brand)
    {
        await _context.Brands.AddAsync(brand);
    }

    public void Remove(Brand brand)
    {
        _context.Brands.Remove(brand);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Brands.AnyAsync(b => b.Name == name);
    }

    public async Task<bool> ExistsByNameExcludingIdAsync(string name, Guid excludeId)
    {
        return await _context.Brands.AnyAsync(b => b.Name == name && b.Id != excludeId);
    }

    public async Task<bool> HasRelatedProductsAsync(Guid brandId)
    {
        return await _context.Products.AnyAsync(p => p.BrandId == brandId);
    }

    public async Task<bool> HasRelatedBrandCategoriesAsync(Guid brandId)
    {
        return await _context.BrandCategories.AnyAsync(bc => bc.BrandId == brandId);
    }
}


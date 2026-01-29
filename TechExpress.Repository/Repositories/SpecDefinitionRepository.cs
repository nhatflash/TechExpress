using Microsoft.EntityFrameworkCore;
using TechExpress.Repository.Contexts;
using TechExpress.Repository.Models;

namespace TechExpress.Repository.Repositories;

public class SpecDefinitionRepository
{
    private readonly ApplicationDbContext _context;
    public SpecDefinitionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpecDefinition?> FindByIdAsync(Guid id)
    {
        return await _context.SpecDefinitions
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
    }

    public async Task<SpecDefinition?> FindByIdWithTrackingAsync(Guid id)
    {
        return await _context.SpecDefinitions
            .AsTracking()
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<(List<SpecDefinition> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchName,
        DateTimeOffset? createdFrom,
        DateTimeOffset? createdTo)
    {
        var query = _context.SpecDefinitions
            .Where(s => !s.IsDeleted)
            .Include(s => s.Category)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchName))
        {
            var keyword = searchName.Trim();
            query = query.Where(s => EF.Functions.Like(s.Name, $"%{keyword}%"));
        }

        if (createdFrom.HasValue)
        {
            query = query.Where(s => s.CreatedAt >= createdFrom.Value);
        }

        if (createdTo.HasValue)
        {
            query = query.Where(s => s.CreatedAt <= createdTo.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(s => s.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<SpecDefinition>> GetAllAsync()
    {
        return await _context.SpecDefinitions
            .Where(s => !s.IsDeleted)
            .Include(s => s.Category)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(SpecDefinition specDefinition)
    {
        await _context.SpecDefinitions.AddAsync(specDefinition);
    }

    public void Update(SpecDefinition specDefinition)
    {
        _context.SpecDefinitions.Update(specDefinition);
    }

    public void Remove(SpecDefinition specDefinition)
    {
        _context.SpecDefinitions.Remove(specDefinition);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.SpecDefinitions
            .AnyAsync(s => s.Name == name && !s.IsDeleted);
    }

    public async Task<bool> ExistsByNameExcludingIdAsync(string name, Guid excludeId)
    {
        return await _context.SpecDefinitions
            .AnyAsync(s => s.Name == name && s.Id != excludeId && !s.IsDeleted);
    }

    public async Task<bool> HasRelatedProductSpecValuesAsync(Guid specDefinitionId)
    {
        return await _context.ProductSpecValues
            .AnyAsync(psv => psv.SpecDefinitionId == specDefinitionId);
    }

    public async Task<bool> CategoryExistsAsync(Guid categoryId)
    {
        return await _context.Categories
            .AnyAsync(c => c.Id == categoryId && !c.IsDeleted);
    }
}
